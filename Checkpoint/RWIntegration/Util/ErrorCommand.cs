using System;

namespace Checkpoint.RWIntegration
{
    public class ErrorCommand
    {
        
        public const int SUCESSO = 0x54;
        public const int ERRO_BCC = 0x01;
        public const int DADOS_OK = 0x45;
        public const int DADOS_COM_ERRO = 0x96;
        public const int ADICIONAR = 0x80;
        public const int ADICIONAR_SUBSTITUIR = 0x57;
        public const int EXCLUIR = 0x78;
        public const int IDENTIFICADOR_INCONSISTENTE = 0x10;
        public const int IDENTIFICADOR_RECUSADO = 0x11;
        public const int CODIGO_RECUSADO = 0x12;
        public const int ESPACO_INSUFICIENTE = 0x13;
        public const int TODAS_TEMPLATE_USUARIO = 0x06;
        public const int UNICA_TEMPLATE_USUARIO = 0x07;

        
        public const int HORA_INVALIDA = 2;
        public const int PARAMETRO_TAMANHO_FLAGERROR_NAO_SUPORTADOS = 3;
        public const int COMANDO_NAO_SUPORTADO_OU_DESCONHECIDO = 4;
        public const int ERRO_NAO_ESPECIFICADO = 5;
        public const int PIS_INEXISTENTE = 8;
        public const int FRAME_COM_ERRO = 96;
        public const int RELOGIO_NAO_AJUSTADO = 203;
        public const int HVERAO_NAO_AJUSTADO = 204;
        public const int NENHUM_FUNCIONARIO_CADASTRADO = 205;
        public const int NENHUMA_MARCACAO_PONTO = 206;
        public const int COMUNICACAO_NAO_ESTABELECIDA = 200;
        public const int RETORNO_INCONSISTENTE = 201;
        public const int OCORREU_EXCECAO = 202;
        public const int ERRO_FINALIZAR_CONEXAO = 400;
        public const int FIM_LEITURA_MARCACOES = 500;

        private int erro;
        private String mensagem;

        public ErrorCommand()
        {
            this.setErro(SUCESSO);
        }

        public ErrorCommand(int erro)
        {
            this.setErro(erro);
        }

        public void setErro(int erro)
        {
            this.erro = erro;
            this.setMensagem();
        }

        public int getErro()
        {
            return erro;
        }

        public String getErroStrHex()
        {
            String erroHex = erro.ToString("X");
            if (erroHex.Length < 2)
            {
                erroHex = "0" + erroHex;
            }
            return "0x" + erroHex;
        }

        public String getErroStrHex(int erro)
        {
            String erroHex = erro.ToString("X");
            if (erroHex.Length < 2)
            {
                erroHex = "0" + erroHex;
            }
            return "0x" + erroHex;
        }

        public String getMensagem()
        {
            return mensagem;
        }

        public String getMensagem(int erro)
        {
            switch (erro)
            {
                case SUCESSO:
                    return "Sucesso!";

                case FIM_LEITURA_MARCACOES:
                    return "Sucesso!";

                case HORA_INVALIDA:
                    return "Hora inválida!";

                case ERRO_BCC:
                    return "Erro de BCC!";

                case COMUNICACAO_NAO_ESTABELECIDA:
                    return "Comunicação não estabelecida!";

                case RETORNO_INCONSISTENTE:
                    return "Retorno Inconsistente!";

                case OCORREU_EXCECAO:
                    return "Ocorreu exceção durante a operação!";

                case DADOS_OK:
                    return "Sucesso!";

                case COMANDO_NAO_SUPORTADO_OU_DESCONHECIDO:
                    return "Comando não suportado ou desconhecido!";

                case ERRO_NAO_ESPECIFICADO:
                    return "Erro não especificado!";

                case PIS_INEXISTENTE:
                    return "PIS não encontrado!";

                case IDENTIFICADOR_INCONSISTENTE:
                    return "Identificador inconsistente!";

                case IDENTIFICADOR_RECUSADO:
                    return "Funcionário não cadastrado no relógio!";

                case CODIGO_RECUSADO:
                    return "Código recusado!";

                case ESPACO_INSUFICIENTE:
                    return "Espaço insuficiente!";

                case FRAME_COM_ERRO:
                    return "Frame recebido com erro!";

                case RELOGIO_NAO_AJUSTADO:
                    return "Relógio não ajustado!";

                case HVERAO_NAO_AJUSTADO:
                    return "Horário de verão não ajustado!";

                case NENHUM_FUNCIONARIO_CADASTRADO:
                    return "Nenhum funcionário cadastrado!";

                case NENHUMA_MARCACAO_PONTO:
                    return "Nenhuma marcação de ponto recebida!";

                case ERRO_FINALIZAR_CONEXAO:
                    return "Erro ao Finalizar Conexão!";

                default:
                    return "Erro desconhecido!";
            }
        }

        private void setMensagem()
        {
            switch (erro)
            {
                case SUCESSO:
                    mensagem = "Sucesso!";
                    break;
                case FIM_LEITURA_MARCACOES:
                    mensagem = "Sucesso!";
                    break;
                case HORA_INVALIDA:
                    mensagem = "Hora inválida!";
                    break;
                case ERRO_BCC:
                    mensagem = "Erro de BCC!";
                    break;
                case COMUNICACAO_NAO_ESTABELECIDA:
                    mensagem = "Comunicação não estabelecida!";
                    break;
                case RETORNO_INCONSISTENTE:
                    mensagem = "Retorno Inconsistente!";
                    break;
                case OCORREU_EXCECAO:
                    mensagem = "Ocorreu exceção durante a operação!";
                    break;
                case DADOS_OK:
                    mensagem = "Sucesso!";
                    break;
                case COMANDO_NAO_SUPORTADO_OU_DESCONHECIDO:
                    mensagem = "Comando não suportado ou desconhecido!";
                    break;
                case ERRO_NAO_ESPECIFICADO:
                    mensagem = "Erro não especificado!";
                    break;
                case PIS_INEXISTENTE:
                    mensagem = "PIS não encontrado!";
                    break;
                case IDENTIFICADOR_INCONSISTENTE:
                    mensagem = "Identificador inconsistente!";
                    break;
                case IDENTIFICADOR_RECUSADO:
                    mensagem = "Funcionário não cadastrado no relógio!";
                    break;
                case CODIGO_RECUSADO:
                    mensagem = "Código recusado!";
                    break;
                case ESPACO_INSUFICIENTE:
                    mensagem = "Espaço insuficiente!";
                    break;
                case FRAME_COM_ERRO:
                    mensagem = "Frame recebido com erro!";
                    break;
                case RELOGIO_NAO_AJUSTADO:
                    mensagem = "Relógio não ajustado!";
                    break;
                case HVERAO_NAO_AJUSTADO:
                    mensagem = "Horário de verão não ajustado!";
                    break;
                case NENHUM_FUNCIONARIO_CADASTRADO:
                    mensagem = "Nenhum funcionário cadastrado!";
                    break;
                case NENHUMA_MARCACAO_PONTO:
                    mensagem = "Nenhuma marcação de ponto recebida!";
                    break;
                case ERRO_FINALIZAR_CONEXAO:
                    mensagem = "Erro ao Finalizar Conexão!";
                    break;
                default:
                    mensagem = "Erro desconhecido!";
                    break;
            }
        }
    }
}
