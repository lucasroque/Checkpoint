using Checkpoint.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checkpoint.RWIntegration.Commands
{
    class CommandSendEmployees : Command
    {
        private ErrorCommand errorCommand;
        private Tcp tcp;
        private String cpf;

        public ErrorCommand execute(String ip, String porta, List<Employee> employees)
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
                    int totalFuncionarios = employees.Count();
                    int funcionarioAtual = 1;
                    int flagErro = ErrorCommand.SUCESSO;
                    while (funcionarioAtual <= totalFuncionarios  && (ErrorCommand.SUCESSO == errorCommand.getErro() || ErrorCommand.DADOS_OK == errorCommand.getErro()))
                    {
                        Employee employee = employees[funcionarioAtual - 1];
                        // Exclusão do funcionário antes de adicioná-lo no REP
                        CommandDeleteEmployee excluiFuncionario = new CommandDeleteEmployee(tcp);

                        errorCommand = excluiFuncionario.execute(ip, porta, cpf, employee.pisPasep);
                        // Envio do funcionário para o REP
                        if (flagErro == ErrorCommand.SUCESSO || flagErro == ErrorCommand.IDENTIFICADOR_RECUSADO || flagErro == ErrorCommand.PIS_INEXISTENTE)
                        {
                            byte[] bytesCPF = Conversor.cpfToByte(cpf);
                            byte[] cabecalhoDadosGravaFuncionario = Command.criarPacoteCabecalho(CommandCodes.START_PC, CommandCodes.ENVIAR_FUNCIONARIO,
                                                                                                 new byte[] { 0x00, 0x00, 0x00, 0x01 },
                                                                                                 new byte[4], bytesCPF,
                                                                                                 (byte)ErrorCommand.ADICIONAR, CommandCodes.END);
                            // Envia o cabeçalho
                            Command.sendBuffer(cabecalhoDadosGravaFuncionario, true, tcp);
                            // Lê e trata 1º cabeçalho
                            lerEtratarResposta(Protocol.QTD_BYTES_CABECALHO_CRIPTOGRAFADO);
                            if (errorCommand.getErro() == ErrorCommand.SUCESSO)
                            {
                                cabecalhoDadosGravaFuncionario = Command.criarPacoteCabecalho(CommandCodes.START_PC, CommandCodes.ENVIAR_FUNCIONARIO,
                                                                                              new byte[] { 0x00, 0x01, 0x00, 0x01 },
                                                                                              new byte[] { 0x00, 0x00, 0x00, 0x61 },
                                                                                              bytesCPF, (byte)ErrorCommand.DADOS_OK, CommandCodes.END);

                                byte checkSumCabecalho = cabecalhoDadosGravaFuncionario[Protocol.QTD_BYTES_CABECALHO_DADOS - 2];
                                byte[] dadosGravaFuncionario = criaPacoteDados(checkSumCabecalho, employee);

                                // Envia os dados do funcionário
                                Command.sendBuffer(ProtocolUtils.merge(cabecalhoDadosGravaFuncionario, dadosGravaFuncionario), true, tcp);
                                // Lê e trata 2ª resposta do REP
                                lerEtratarResposta(Protocol.QTD_BYTES_CABECALHO_CRIPTOGRAFADO);
                            }
                        }
                        funcionarioAtual++;
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
		if (respostaREP != null) {
			qtdBytesRecebidos = respostaREP.Length;
		}

        // Trata a resposta do REP
        errorCommand = tratarResposta(CommandCodes.ENVIAR_FUNCIONARIO, respostaREP, qtdBytesRecebidos, qtdBytesEsperada);

            return respostaREP;
	}

    private byte[] criaPacoteDados(byte checkSumCabecalho, Employee employee)
    {
        byte[] nome = Conversor.stringToByteArray(employee.employeeName, 52); // Campo Nome 52 bytes;
        byte[] pis = Conversor.pisToByte(Conversor.SomenteNumeros(employee.pisPasep)); // Campo PIS 6 bytes;
        byte[] reservado = new byte[4]; // Campo Reservado 4 bytes;
        String cartaoString = "";
        byte[] cartao = Conversor.stringParaBytes(cartaoString != null && cartaoString.Length > 0 ? cartaoString : "0", 20); // Campo Cartão 20 bytes;
        byte[] codigo = Conversor.intToByteArray(employee.idEmployee, 3); // Campo Código 3 bytes;
        byte[] mestre = new byte[1]; // Campo Mestre 1 byte;
        //String senhaStr = funcionario.getSenha();
        byte[] senha = new byte[3];
        /*if (senhaStr != null && !senhaStr.equals("000000") && senhaStr.length() < 7)
        {
            senha = funcionario.getSenhaArray(); // Campo Senha 3 bytes;
        }*/
        byte[] verifica1ToN = new byte[1]; // Campo Verifica 1 byte;
        byte[] cmp = new byte[1]; // Campo adicional 1 byte;
        byte[] requisicao = new byte[] { };
        requisicao = ProtocolUtils.merge(nome, pis, reservado, cartao, codigo, mestre, senha, verifica1ToN, cmp);
        byte checksum = ProtocolUtils.getChecksum(ProtocolUtils.merge(requisicao, new byte[] { checkSumCabecalho, Convert.ToByte(CommandCodes.END) }));
        requisicao = ProtocolUtils.merge(requisicao, new byte[] { checksum });
        return requisicao;
    }

    }
}
