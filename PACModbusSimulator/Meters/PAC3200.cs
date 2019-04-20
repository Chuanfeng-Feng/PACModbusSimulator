﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace PACModbusSimulator
{
    public class PAC3200 : StandardPAC
    {
        //String representing PAC3200 device type
        public const string TypeString = "PAC3200";

        /// <summary>
        /// PAC3200 meter
        /// </summary>
        /// <param name="simulator">Simulator object</param>
        /// <param name="name">Name of PAC meter</param>
        /// <param name="portNumber">Port number</param>
        /// <param name="nominalCurrent">Nominal current</param>
        /// <param name="nominalPowerFactor">Nominal power factor</param>
        public PAC3200(PACSimulator simulator, string name = "", Int32 portNumber = 502, float nominalCurrent = 100, float nominalPowerFactor = 0.8f) : base(simulator, name, portNumber, nominalCurrent,nominalPowerFactor)
        {
        }

        /// <summary>
        /// Method for getting string representing device type
        /// </summary>
        /// <returns></returns>
        public override string GetTypeName()
        {
            return TypeString;
        }
    }
}