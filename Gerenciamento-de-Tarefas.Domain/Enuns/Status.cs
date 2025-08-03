using System;
using System.ComponentModel;

namespace Gerenciamento_de_Tarefas.Domain.Enuns
{
    public enum Status
    {
        [Description("Pendente")]
        Pendente,

        [Description("Em Andamento")]
        EmAndamento,

        [Description("Concluída")]
        Concluida,

        [Description("Cancelada")]
        Cancelada

              
    }
}
