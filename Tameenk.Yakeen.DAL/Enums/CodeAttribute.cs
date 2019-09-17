using System;
using System.Collections.Generic;
using System.Text;

namespace Tameenk.Yakeen.DAL.Enums
{
    public class CodeAttribute : Attribute
    {
        #region Ctor

        /// <summary>
        /// Initailize new instance of CodeAttribute class
        /// with code parameter.
        /// </summary>
        /// <param name="code">The code.</param>
        public CodeAttribute(string code)
        {
            Code = code;
        }
        #endregion

        #region Properties
        public string Code { get; private set; }

        #endregion
    }
}
