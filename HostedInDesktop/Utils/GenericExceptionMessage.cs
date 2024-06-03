using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HostedInDesktop.Utils
{

    public enum ExceptionMessages
    {
        GENERIC_DESKTOP_EXCEPTION_MEESAGE
    }

    public static class GenericExceptionMessage
    {
        private static readonly Dictionary<ExceptionMessages, string> StatusDescriptions = new Dictionary<ExceptionMessages, string>
        {
            { ExceptionMessages.GENERIC_DESKTOP_EXCEPTION_MEESAGE, "Se ha producido un contratiempo al realizar la petición. Por favor, intentelo de nuevo mas tarde o " +
                "ponte en contacto con el soporte tecnico."}
        };

        public static string GetDescription(this ExceptionMessages status)
        {
            return StatusDescriptions[status];
        }
    }
}
