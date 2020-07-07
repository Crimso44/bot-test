using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eve
{
    public static class Css
    {
        public const string Message = @"
            body {
                font-family: Microsoft Sans Serif;
                font-size: 12px;
                font-weight: normal;
                font-style: normal;
                font-stretch: normal;
                line-height: 1.33;
                letter-spacing: normal;
                text-align: left;
                color: #393c40;
                margin: 0;
            }
            button {
                width: 100%;
                cursor: pointer;
                margin: 2px;
                font-style: normal;
                font-weight: normal;
                font-size: 12px;
                background: #FFFFFF;
                box-shadow: 0px 2px 10px rgba(27, 65, 173, 0.2);
                border-radius: 4px;
                color: #1B41AD;
                padding: 7px 16px 8px 16px;
                text-align: left;
                border: 1px rgba(27, 65, 173, 0.4);
                line-height: 16px;
                letter-spacing: 0.25px;
                max-width: 300px;
            }
            button:hover {
                    box-shadow: 0px 2px 10px rgba(27, 65, 173, 0.4);
            }

            a {
                color: #1B41AD;
                text-decoration: none;
                font-weight: bold;
            }

            .color-red {
                color: red;
            }
        ";
    }
}
