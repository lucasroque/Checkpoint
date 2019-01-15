using Checkpoint.Model;
using System;
using System.Collections.Generic;

namespace Checkpoint.RWIntegration.Commands
{
    class CommandReadEmployees : Command
    {
        private Employee funcionarioLido;
        private String hash;
        private Tcp tcp;
        private ErrorCommand errorCommand;
        private List<Employee> listaFuncionariosLidos;
        private List<String> listaPisNaoEncontrados;
        private int qtdPisASerLido;

        private void leFuncionarioPorPIS(String pis)
        {
            byte[] buffer = criarPacoteCabecalho(CommandCodes.START_PC, CommandCodes.LER_FUNCIONARIO,
                                                         Conversor.pisToByte(pis), new byte[2], new byte[6], (byte)0x00, CommandCodes.END);
            sendBuffer(buffer, true, tcp);
            // Lê e trata 1º cabeçalho
            byte[] respostaREP = this.lerEtratarResposta(Protocol.QTD_BYTES_CABECALHO_CRIPTOGRAFADO);
            if (errorCommand.getErro() == ErrorCommand.SUCESSO)
            {
                errorCommand.setErro(ErrorCommand.DADOS_OK);
                // Lê e trata cabeçalho dos dados
                respostaREP = this.lerEtratarResposta(Protocol.QTD_BYTES_FUNCIONARIO);
                if (errorCommand.getErro() == ErrorCommand.DADOS_OK)
                {
                    // Lê os dados do funcionário 
                    respostaREP = ProtocolUtils.copyOfRange(respostaREP, Protocol.QTD_BYTES_CABECALHO_DADOS, respostaREP.Length);
                    int qtdBytesRecebidos = -1;
                    if (respostaREP != null)
                    {
                        qtdBytesRecebidos = respostaREP.Length;
                    }
                    if (Protocol.QTD_BYTES_FUNCIONARIO - Protocol.QTD_BYTES_CABECALHO_DADOS == qtdBytesRecebidos)
                    {
                        errorCommand = this.tratarResposta(0, respostaREP, 0, 0);
                        // Envia pacote de confirmação OK da leitura de funcionário
                        buffer = criarPacoteCabecalho(CommandCodes.START_PC, CommandCodes.LER_FUNCIONARIO,
                                                                      new byte[] { 0x00, (byte)1, 0x00, (byte)1 }, new byte[4],
                                                                      new byte[6], (byte)ErrorCommand.DADOS_OK, CommandCodes.END);
                        sendBuffer(buffer, true, tcp);
                        if (ErrorCommand.SUCESSO == errorCommand.getErro() || ErrorCommand.DADOS_OK == errorCommand.getErro())
                        {
                            listaFuncionariosLidos.Add(funcionarioLido);
                        }
                    }
                }
            }
            else if (errorCommand.getErro() == ErrorCommand.PIS_INEXISTENTE)
            {
                listaPisNaoEncontrados.Add(pis);
            }
        }

        private void leTodosFuncionarios()
        {
            byte[] buffer = criarPacoteCabecalho(CommandCodes.START_PC, CommandCodes.LER_FUNCIONARIO, new byte[6],
                                                         new byte[2], new byte[6], (byte)0x00, CommandCodes.END);
            sendBuffer(buffer, true, tcp);
            // Lê e trata 1º cabeçalho
            byte[] respostaREP = this.lerEtratarResposta(Protocol.QTD_BYTES_CABECALHO_CRIPTOGRAFADO);
            if (errorCommand.getErro() == ErrorCommand.SUCESSO)
            {
                int qtdFuncionarios = Conversor.ByteArrayToint(ProtocolUtils.copyOfRange(respostaREP,
                                                                                       Protocol.INDICE_INICIO_PARAMETRO,
                                                                                       Protocol.INDICE_FIM_PARAMETRO + 1));
                int i = 1;
                byte[] total;
                byte[] atual;
                while (i <= qtdFuncionarios
                        && (ErrorCommand.SUCESSO == errorCommand.getErro() || ErrorCommand.DADOS_OK == errorCommand.getErro()))
                {
                    // Lê e trata cabeçalho dos dados
                    respostaREP = this.lerEtratarResposta(Protocol.QTD_BYTES_FUNCIONARIO);
                    if (errorCommand.getErro() == ErrorCommand.DADOS_OK)
                    {
                        // Lê os dados do funcionário 
                        respostaREP = ProtocolUtils.copyOfRange(respostaREP, Protocol.QTD_BYTES_CABECALHO_DADOS, respostaREP.Length);
                        int qtdBytesRecebidos = -1;
                        if (respostaREP != null)
                        {
                            qtdBytesRecebidos = respostaREP.Length;
                        }
                        if (Protocol.QTD_BYTES_FUNCIONARIO - Protocol.QTD_BYTES_CABECALHO_DADOS == qtdBytesRecebidos)
                        {
                            errorCommand = this.tratarResposta(0, respostaREP, 0, 0);
                            if (ErrorCommand.SUCESSO == errorCommand.getErro() || ErrorCommand.DADOS_OK == errorCommand.getErro())
                            {
                                listaFuncionariosLidos.Add(funcionarioLido);
                            }
                        }
                        // Envia pacote de confirmação OK da leitura de funcionário
                        total = Conversor.intToByteArray2(qtdFuncionarios);
                        atual = Conversor.intToByteArray2(i);
                        buffer = criarPacoteCabecalho(CommandCodes.START_PC, CommandCodes.LER_FUNCIONARIO,
                                                                      new byte[] { total[0], total[1], atual[0], atual[1] }, new byte[4],
                                                                      new byte[6], (byte)ErrorCommand.DADOS_OK, CommandCodes.END);
                        sendBuffer(buffer, true, tcp);
                    }
                    i++;
                }
            }
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
            errorCommand = tratarResposta(CommandCodes.LER_FUNCIONARIO, respostaREP, qtdBytesRecebidos, qtdBytesEsperada);
		    return respostaREP;
	    }

        public override ErrorCommand tratarResposta(int codigoComandoEsperado, byte[] dados, int qdeRecebida, int qdeEsperada)
        {
            ErrorCommand errorCommand = new ErrorCommand(ErrorCommand.SUCESSO);
            try
            {
                funcionarioLido = new Employee();
                // Conversão do Nome
                byte[] bytesNome = new byte[Protocol.TAMANHO_NOME_FUNCIONARIO];
                for (int i = 0, j = Protocol.INDICE_NOME_FUNCIONARIO;
                         i < Protocol.TAMANHO_NOME_FUNCIONARIO &&
                         j < Protocol.INDICE_NOME_FUNCIONARIO + Protocol.TAMANHO_NOME_FUNCIONARIO;
                         i++, j++)
                {
                    bytesNome[i] = dados[j];
                }
                funcionarioLido.employeeName = Conversor.bytesASCIIToString(bytesNome);

                // Conversão do PIS
                byte[] bytesPIS = new byte[Protocol.TAMANHO_PIS_FUNCIONARIO];
                for (int i = 0, j = Protocol.INDICE_PIS_FUNCIONARIO;
                         i < Protocol.TAMANHO_PIS_FUNCIONARIO &&
                         j < Protocol.INDICE_PIS_FUNCIONARIO + Protocol.TAMANHO_PIS_FUNCIONARIO;
                         i++, j++)
                {
                    bytesPIS[i] = dados[j];
                }
                funcionarioLido.pisPasep = Conversor.BCDtoString(bytesPIS);

                // Conversão do ID BIO
                byte[] bytesIDBio = new byte[Protocol.TAMANHO_ID_BIO];
                for (int i = 0, j = Protocol.INDICE_ID_BIO;
                         i < Protocol.TAMANHO_ID_BIO &&
                         j < Protocol.INDICE_ID_BIO + Protocol.TAMANHO_ID_BIO;
                         i++, j++)
                {
                    bytesIDBio[i] = dados[j];
                }

                // Conversão do Cartão
                byte[] bytesCartao = new byte[Protocol.TAMANHO_CARTAO_FUNCIONARIO];
                for (int i = 0, j = Protocol.INDICE_CARTAO_FUNCIONARIO;
                         i < Protocol.TAMANHO_CARTAO_FUNCIONARIO &&
                         j < Protocol.INDICE_CARTAO_FUNCIONARIO + Protocol.TAMANHO_CARTAO_FUNCIONARIO;
                         i++, j++)
                {
                    bytesCartao[i] = dados[j];
                }
                Conversor.bytesASCIIToString(bytesCartao);

                // Conversão do Código
                byte[] bytesCodigo = new byte[Protocol.TAMANHO_CODIGO_FUNCIONARIO];
                for (int i = 0, j = Protocol.INDICE_CODIGO_FUNCIONARIO;
                         i < Protocol.TAMANHO_CODIGO_FUNCIONARIO &&
                         j < Protocol.INDICE_CODIGO_FUNCIONARIO + Protocol.TAMANHO_CODIGO_FUNCIONARIO;
                         i++, j++)
                {
                    bytesCodigo[i] = dados[j];
                }
                funcionarioLido.idEmployee = Convert.ToInt32(Conversor.bytesCodigoToString(bytesCodigo));

                // Conversão do Mestre
                Conversor.byteToInt(dados[Protocol.INDICE_MESTRE_FUNCIONARIO]);

                // Conversão da Senha
                byte[] bytesSenha = new byte[Protocol.TAMANHO_SENHA_FUNCIONARIO];
                for (int i = 0, j = Protocol.INDICE_SENHA_FUNCIONARIO;
                         i < Protocol.TAMANHO_SENHA_FUNCIONARIO &&
                         j < Protocol.INDICE_SENHA_FUNCIONARIO + Protocol.TAMANHO_SENHA_FUNCIONARIO;
                         i++, j++)
                {
                    bytesSenha[i] = dados[j];
                }
                Conversor.BCDtoString(bytesSenha);

                // Conversão do Verificar 1 pra N
                Conversor.byteToInt(dados[Protocol.INDICE_VERIFICAR_1_PRA_N]);

            }
            catch (Exception e)
            {
                errorCommand.setErro(ErrorCommand.RETORNO_INCONSISTENTE);
            }

            return errorCommand;
        }
    }
}
