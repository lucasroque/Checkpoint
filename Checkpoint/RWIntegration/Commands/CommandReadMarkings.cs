using Checkpoint.Model;
using System;
using System.Collections.Generic;
using System.IO;

namespace Checkpoint.RWIntegration.Commands
{
    class CommandReadMarkings : Command
    {

        private Tcp tcp;
        private ErrorCommand errorCommand;
        private Marking marking;
        private LinkedList<Marking> listaMarcacoes;
        private HashSet<Int16> setNsr = new HashSet<Int16>();
        private int totalPacotes = 0;

        public LinkedList<Marking> getListaMarcacoes()
        {
            return listaMarcacoes;
        }

        public ErrorCommand execute(String ip, String porta, int nsr)
        {
            errorCommand = new ErrorCommand();
            tcp = null;
            try
            {
                tcp = new Tcp(ip, Convert.ToInt16(porta));
            }
            catch (IOException e) {
                errorCommand.setErro(ErrorCommand.COMUNICACAO_NAO_ESTABELECIDA);
                return errorCommand;
            }
            try
            {
                try
                {
                    /** Quando é informado um NSR maior do que o último gravado no REP,
                     * o REP retorna o flag 0x96 (O Frame recebido contém erro).
                     **/
                    byte[] buffer = criarPacoteCabecalho(CommandCodes.START_PC, CommandCodes.LER_MARCACAO,
                                                                 Conversor.intToByteArray(nsr, 4),
                                                                 Conversor.intToByteArray(Protocol.TAMANHO_PACOTE_MARCACOES, 4),
                                                                  new byte[6], (byte)0x00, CommandCodes.END);
                    sendBuffer(buffer, true, tcp);
                    // Lê e trata 1º cabeçalho
                    byte[] respostaREP = this.lerEtratarResposta(Protocol.QTD_BYTES_CABECALHO_CRIPTOGRAFADO);
                    byte[] totalMarcacoes = ProtocolUtils.copyOfRange(respostaREP, Protocol.INDICE_INICIO_PARAMETRO, Protocol.INDICE_INICIO_PARAMETRO + 4);
                    int totalMarcacoesInt = Conversor.ByteArrayToint(totalMarcacoes);
                    // Cálculo necessário devido ao último pacote ser de tamanho variável
                    int qtdMarcacoesUltimoPacote = totalMarcacoesInt % Protocol.TAMANHO_PACOTE_MARCACOES;
                    int qtdBytesASeremLidos = 0;
                    if (errorCommand.getErro() == ErrorCommand.SUCESSO)
                    {
                        errorCommand.setErro(ErrorCommand.DADOS_OK);
                        int pacoteAtual = -1;
                        byte[] atual;
                        listaMarcacoes = new LinkedList<Marking>();
                        while (pacoteAtual < totalPacotes
                                && (ErrorCommand.SUCESSO == errorCommand.getErro() || ErrorCommand.DADOS_OK == errorCommand.getErro()))
                        {
                            // Lê e trata cabeçalho dos dados
                            if (totalMarcacoesInt < Protocol.TAMANHO_PACOTE_MARCACOES
                                    || (pacoteAtual != -1 && (pacoteAtual + 1) == totalPacotes && qtdMarcacoesUltimoPacote > 0))
                            { // último pacote a ser recebido
                                //qtdBytesASeremLidos = AES.defineTamanhoPacote(Protocol.QTD_BYTES_CABECALHO_DADOS + Protocol.QTD_BYTES_MARCACAO * qtdMarcacoesUltimoPacote);
                            }
                            else
                            {
                                qtdBytesASeremLidos = Protocol.QTD_BYTES_PACOTES_MARCACOES;
                            }
                            respostaREP = this.lerEtratarResposta(qtdBytesASeremLidos);
                            if (errorCommand.getErro() == ErrorCommand.DADOS_OK)
                            {
                                // Verificando o total de pacotes a serem lidos do REP
                                byte[] total = ProtocolUtils.copyOfRange(respostaREP, Protocol.INDICE_INICIO_PARAMETRO, Protocol.INDICE_INICIO_PARAMETRO + 2);
                                totalPacotes = Conversor.ByteArrayToint(total);
                                // Verificando o pacote atual
                                atual = ProtocolUtils.copyOfRange(respostaREP, Protocol.INDICE_FIM_PARAMETRO - 1, Protocol.INDICE_FIM_PARAMETRO + 1);
                                pacoteAtual = Conversor.ByteArrayToint(atual);

                                // Lê os pacotes de marcação de ponto 
                                respostaREP = ProtocolUtils.copyOfRange(respostaREP, Protocol.QTD_BYTES_CABECALHO_DADOS, respostaREP.Length);
                                int qtdBytesRecebidos = -1;
                                if (respostaREP != null)
                                {
                                    qtdBytesRecebidos = respostaREP.Length;
                                }
                                if (qtdBytesASeremLidos - Protocol.QTD_BYTES_CABECALHO_DADOS == qtdBytesRecebidos)
                                {
                                    errorCommand = this.tratarResposta(0, respostaREP, 0, 0);
                                }
                                if (errorCommand.getErro() == ErrorCommand.FIM_LEITURA_MARCACOES)
                                {
                                    atual[0] = total[0];
                                    atual[1] = total[1];
                                }
                                // Envia comando de leitura do(s) próximo(s) pacote(s) de marcação de ponto
                                buffer = criarPacoteCabecalho(CommandCodes.START_PC, CommandCodes.LER_MARCACAO,
                                                                              new byte[] { total[0], total[1], atual[0], atual[1] }, new byte[4],
                                                                              new byte[6], (byte)ErrorCommand.DADOS_OK, CommandCodes.END);

                                sendBuffer(buffer, true, tcp);
                            }
                        }
                        if (errorCommand.getErro() == ErrorCommand.DADOS_OK
                                || errorCommand.getErro() == ErrorCommand.FIM_LEITURA_MARCACOES)
                        {
                            errorCommand.setErro(ErrorCommand.SUCESSO);
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

        private byte[] lerEtratarResposta(int qtdBytesEsperada)
        {
            // Lê resposta do REP
            byte[] respostaREP = readAnswer(tcp, qtdBytesEsperada);
            int qtdBytesRecebidos = -1;
            if (respostaREP != null)
            {
                qtdBytesRecebidos = respostaREP.Length;
            }
            // Trata a resposta do REP
            errorCommand = tratarResposta(CommandCodes.LER_MARCACAO, respostaREP, qtdBytesRecebidos, qtdBytesEsperada);
            return respostaREP;
        }
        
        /*public override ErrorCommand tratarResposta(int codigoComandoEsperado, byte[] dados, int qdeRecebida, int qdeEsperada)
        {
            ErrorCommand errorCommand = new ErrorCommand(ErrorCommand.SUCESSO);
            try
            {
                int inicioCampo;
                int fimCampo;
                int offset;
                int qtdBytes = dados.Length;
                int ultimoByte = dados[qtdBytes - 1];
                while (ultimoByte == -1 || ultimoByte == 0)
                {
                    dados = ProtocolUtils.copyOfRange(dados, 0, qtdBytes - 1);
                    qtdBytes = dados.Length;
                    ultimoByte = dados[qtdBytes - 1];
                }
                int qtdMarcacoes = qtdBytes / Protocol.QTD_BYTES_MARCACAO;
                for (int i = 0; i < qtdMarcacoes; i++)
                {
                    marking = new Marking();
                    offset = Protocol.QTD_BYTES_MARCACAO * i;
                    inicioCampo = 0 + offset;
                    fimCampo = 4 + offset;
                    marking.setNumEvento(Conversor.ByteArrayToint(ProtocolUtils.copyOfRange(dados, inicioCampo, fimCampo))); // 4 bytes
                    inicioCampo = 4 + offset;
                    fimCampo = 8 + offset;
                    marking.setNsr(Conversor.ByteArrayToint(ProtocolUtils.copyOfRange(dados, inicioCampo, fimCampo))); // 4 bytes
                    inicioCampo = 8 + offset;
                    fimCampo = 13 + offset;
                    marking.setPIS(MaskProcessor.zeroFill(Conversor.bytesPISToString(ProtocolUtils.copyOfRange(dados, inicioCampo, fimCampo)), MaskProcessor.PIS_LENGTH)); // 5 bytes
                    inicioCampo = 0 + offset;
                    fimCampo = 4 + offset;
                    marking.setData(new Formatter().format("%02d/%02d/%04d",
                                       Conversor.byteToInt(ProtocolUtils.copyOfRange(dados,
                                                                                   13 + offset,
                                                                                   14 + offset)[0]), // dia 1 byte 
                                       Conversor.byteToInt(ProtocolUtils.copyOfRange(dados,
                                                                                      14 + offset,
                                                                                      15 + offset)[0]), // mês 1 byte 
                                       Conversor.ByteArrayToint(ProtocolUtils.copyOfRange(dados,
                                                                                      15 + offset,
                                                                                      17 + offset))) // ano 2 bytes 
                                                                                        .toString());
                    marking.setHora(new Formatter().format("%02d:%02d:00",
                                       Conversor.byteToInt(ProtocolUtils.copyOfRange(dados,
                                                                                      17 + offset,
                                                                                      18 + offset)[0]), // hora 1 byte
                                       Conversor.byteToInt(ProtocolUtils.copyOfRange(dados,
                                                                                      18 + offset,
                                                                                      19 + offset)[0])) // minuto 1 byte
                                                                                        .toString());
                    if (marking.getNsr() > 0)
                    {

                        if (setNsr.contains(marking.getNsr()))
                        {
                            totalPacotes = -1;
                            errorCommand.setErro(ErrorCommand.FIM_LEITURA_MARCACOES);
                            break;
                        }
                        else
                        {
                            setNsr.add(marking.getNsr());
                            listaMarcacoes.Add(marking);
                        }
                    }
                }

            }
            catch (Exception e)
            {
                errorCommand.setErro(ErrorCommand.RETORNO_INCONSISTENTE);
            }

            return errorCommand;
        }*/

    }
}
