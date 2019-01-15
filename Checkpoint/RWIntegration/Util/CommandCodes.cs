using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checkpoint.RWIntegration
{
    class CommandCodes
    {
        public static readonly int START_PC = 0x0A;
        public static readonly int START_DISPOSITIVO = 0x3D;
        public static readonly int END = 0x40;
        public static readonly int ENVIAR_DATA_HORA = 0x91;
        public static readonly int ENVIAR_HORARIO_VERAO = 0x93;
        public static readonly int LER_HORARIO_VERAO = 0x94;
        public static readonly int LER_DATA_HORA = 0x92;
        public static readonly int LER_EMPREGADOR = 0xAB;
        public static readonly int ENVIAR_EMPREGADOR = 0xBC;
        public static readonly int ENVIAR_FUNCIONARIO = 0xBE;
        public static readonly int LER_FUNCIONARIO = 0xBD;
        public static readonly int ENVIAR_DIGITAL = 0xBF;
        public static readonly int LER_DIGITAL = 0xAD;
        public static readonly int LER_MARCACAO = 0xF0;
        public static readonly int ENVIAR_QTD_PAPEL = 0xAF;
        public static readonly int LER_QTD_PAPEL = 0xB0;
        public static readonly int ENVIAR_QTD_POUCO_PAPEL = 0xB1;
        public static readonly int LER_QTD_POUCO_PAPEL = 0xB2;
        public static readonly int VERIFICAR_PAPEL_ENROSCADO = 0xFE;
    }
}
