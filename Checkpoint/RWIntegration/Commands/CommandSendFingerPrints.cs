using Checkpoint.Model;
using Checkpoint.RWIntegration;
using System;
using System.Collections.Generic;
using System.IO;

namespace Checkpoint.RWIntegration
{
    class CommandSendFingerPrints : Command
    {
        private Tcp tcp;
        private ErrorCommand errorCommand;

        public ErrorCommand execute(String ip, String porta, List<Digital> digitals, String pis)
        {
            errorCommand = new ErrorCommand();
            tcp = null;
            try
            {
                tcp = new Tcp(ip, Convert.ToInt16(porta));
            }
            catch (IOException e1) {
                errorCommand.setErro(ErrorCommand.COMUNICACAO_NAO_ESTABELECIDA);
                return errorCommand;
            }
            int totalDigitais = 0;
            try
            {
                try
                {
                    totalDigitais = digitals.Count;
                    int digitalAtual = 1;
                    byte flag;
                    if (totalDigitais == 1)
                    {
                        flag = ErrorCommand.UNICA_TEMPLATE_USUARIO;
                    }
                    else
                    {
                        flag = ErrorCommand.TODAS_TEMPLATE_USUARIO;
                    }
                    byte[] cabecalhoDadosGravaDigital = criarPacoteCabecalho(CommandCodes.START_PC, CommandCodes.ENVIAR_DIGITAL,
                                                                                     new byte[] { 0x00, 0x00, 0x00, (byte)totalDigitais }, new byte[4],
                                                                                     Conversor.pisToByte(pis),
                                                                                     flag, CommandCodes.END);
                    byte[] primeiroBlocoCabecalho = ProtocolUtils.copyOfRange(cabecalhoDadosGravaDigital, 0, 16); // primeiros 16 bytes do cabeçalho
                                                                                                           // Envia o cabeçalho
                    sendBuffer(primeiroBlocoCabecalho, true, tcp);
                    /**
                     * Envio dos bytes do cabeçalho ainda não enviados + bloco vazio (0xFF).
                     * Isto é necessário para o REP reconhecer tudo corretamente (visto empiricamente). 
                     */
                    Boolean incluirBytesNoInicio = true;
                    enviaBlocoVazio(ProtocolUtils.copyOfRange(cabecalhoDadosGravaDigital, 16, cabecalhoDadosGravaDigital.Length), incluirBytesNoInicio);

                    // Lê 1ª resposta do REP
                    lerEtratarResposta(Protocol.QTD_BYTES_CABECALHO_CRIPTOGRAFADO);
                    if (errorCommand.getErro() == ErrorCommand.SUCESSO)
                    {
                        errorCommand.setErro(ErrorCommand.DADOS_OK);
                        incluirBytesNoInicio = false;
                        for (; (digitalAtual <= totalDigitais) && (errorCommand.getErro() == ErrorCommand.DADOS_OK); digitalAtual++)
                        {
                            cabecalhoDadosGravaDigital = criarPacoteCabecalho(CommandCodes.START_PC, CommandCodes.ENVIAR_DIGITAL,
                                                                                       new byte[] { 0x00, (byte)totalDigitais, 0x00, (byte)digitalAtual },
                                                                                       new byte[] { 0x00, 0x00, 0x03, 0x23 }, // 803 bytes
                                                                                      new byte[6], (byte)ErrorCommand.DADOS_OK, CommandCodes.END);
                            byte checkSumCabecalho = cabecalhoDadosGravaDigital[cabecalhoDadosGravaDigital.Length - 2];
                            Digital digitalComunicador = digitals[digitalAtual - 1];
                            byte[] dadosGravaDigital = criaPacoteDados(checkSumCabecalho, digitalComunicador);

                            /**
                             * Envio de bloco vazio:
                             * Necessário para o REP reconhecer tudo corretamente (visto empiricamente). 
                             */
                            if (digitalAtual == 1)
                            {
                                enviaBlocoVazio(ProtocolUtils.copyOfRange(cabecalhoDadosGravaDigital, 0, 2), incluirBytesNoInicio);
                                // Envia a digital
                                sendBuffer(ProtocolUtils.merge(ProtocolUtils.copyOfRange(cabecalhoDadosGravaDigital, 2, cabecalhoDadosGravaDigital.Length), dadosGravaDigital), true, tcp);
                            }
                            else
                            {
                                primeiroBlocoCabecalho = ProtocolUtils.copyOfRange(cabecalhoDadosGravaDigital, 0, 16); // primeiros 16 bytes 
                                sendBuffer(primeiroBlocoCabecalho, true, tcp);
                                // Envia a digital
                                sendBuffer(ProtocolUtils.merge(ProtocolUtils.copyOfRange(cabecalhoDadosGravaDigital, 16, cabecalhoDadosGravaDigital.Length), dadosGravaDigital), true, tcp);
                            }

                            // Lê e trata 2ª resposta do REP
                            lerEtratarResposta(Protocol.QTD_BYTES_CABECALHO_CRIPTOGRAFADO);
                            incluirBytesNoInicio = true;
                        }
                    }
                }
                catch (Exception e)
                {
                    errorCommand.setErro(ErrorCommand.OCORREU_EXCECAO);
                }
                tcp.finalizaConexao();
            }
            catch (Exception e)
            {
                errorCommand.setErro(ErrorCommand.ERRO_FINALIZAR_CONEXAO);
            }

            return errorCommand;
        }

        private void enviaBlocoVazio(byte[] bytes, Boolean incluirBytesNoInicio)
        {

            byte[] blocoVazio = new byte[AES.QTD_BYTES_BLOCO - bytes.Length];
		    for (int i = 0; i<blocoVazio.Length; i++) {
			    blocoVazio[i] = (byte) 0xFF;
		    }
		    if (incluirBytesNoInicio) {
			    blocoVazio = ProtocolUtils.merge(bytes, blocoVazio);
			    byte[] blocoVazioAux = new byte[AES.QTD_BYTES_BLOCO];
			    for (int i = 0; i<blocoVazioAux.Length; i++) {
				    blocoVazioAux[i] = (byte) 0xFF;
			    }
            blocoVazio = ProtocolUtils.merge(blocoVazio, blocoVazioAux);
		    } else {
			        blocoVazio = ProtocolUtils.merge(blocoVazio, bytes);
		    }
		
		    sendBuffer(blocoVazio, true, tcp);
	    }

        private byte[] lerEtratarResposta(int qtdBytesEsperada)
        {
            // Lê resposta do REP
            byte[] respostaREP = readAnswer(tcp, qtdBytesEsperada);
            int qtdBytesRecebidos = -1;
		    if (respostaREP != null) {
			    qtdBytesRecebidos = respostaREP.Length;
		    }
            // Trata a resposta do REP
            errorCommand = tratarResposta(CommandCodes.ENVIAR_DIGITAL, respostaREP, qtdBytesRecebidos, qtdBytesEsperada);
		    return respostaREP;
	    }

        private byte[] criaPacoteDados(byte checkSumCabecalho, Digital digital)
        {
            byte[] requisicao = new byte[] { 0x01, 0x20, 0x03 }; // bytes fixos
            String dedoAux = digital.template1.Substring(6); // desconsidera os 3 primeiros bytes (fixados acima)
            dedoAux += digital.template2;
            requisicao = ProtocolUtils.merge(requisicao, Conversor.hexStringToByteArray(dedoAux));
            byte checksum = ProtocolUtils.getChecksum(ProtocolUtils.merge(requisicao, new byte[] { checkSumCabecalho, Convert.ToByte(CommandCodes.END) }));
            requisicao = ProtocolUtils.merge(requisicao, new byte[] { checksum });
            return requisicao;
        }
    }
}
