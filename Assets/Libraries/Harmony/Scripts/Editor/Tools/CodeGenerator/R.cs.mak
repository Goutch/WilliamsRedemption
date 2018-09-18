// ----- AUTO GENERATED CODE - ANY MODIFICATION WILL BE OVERRIDEN ----- //
// ----- GENERATED ON ${timeStamp} ----- //
using System;

namespace Harmony
{
    public static class R
    {
        public static class E
        {
        % for constClass in constClasses:
            public enum ${constClass.name}
            {
            % for constMember in constClass.members:
                % if constMember.isValid:
                ${constMember.name} = ${constMember.id}, //In "${constMember.path}".
                % else:
                //${constClass.name} "${constMember.name}" has invalid name. Non-alphanumerical characters are prohibited. In "${constMember.path}".
                % endif
            % endfor
            }
        % endfor
        }
        public static class S
        {
        % for constClass in constClasses:
            public static class ${constClass.name}
            {
            % for constMember in constClass.members:
                % if constMember.isValid:
                public const string ${constMember.name} = "${constMember.value}"; //In "${constMember.path}".
                % else:
                //${constClass.name} "${constMember.name}" has invalid name. Non-alphanumerical characters are prohibited. In "${constMember.path}".
                % endif
            % endfor

                public static string ToString(E.${constClass.name} value)
                {
                    switch (value)
                    {
                    % for constMember in constClass.members:
                        % if constMember.isValid:
                        case E.${constClass.name}.${constMember.name}:
                            return ${constMember.name};
                        % endif
                    % endfor
                    }
                    return null;
                }

                public static E.${constClass.name} ToEnum(string value)
                {
                    switch (value)
                    {
                    % for constMember in constClass.members:
                        % if constMember.isValid:
                        case ${constMember.name}:
                            return E.${constClass.name}.${constMember.name};
                        % endif
                    % endfor
                    }
                    throw new ArgumentException("Unable to convert " + value + " to R.E.${constClass.name}.");
                }
            }
        % endfor
        }
    }
}
