﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace PACModbusSimulator
{
    public class Int16Variable : VariableBase
    {
        /// <summary>
        /// Int16 variable
        /// </summary>
        /// <param name="name">Name of variable</param>
        /// <param name="offset">Offset of variable</param>
        /// <param name="shouldInsertToHR">Should variable be insterted to Holding registers</param>
        /// <param name="shouldInsertToIR">Should variable be insterted to Input registers</param>
        /// <param name="randomGeneration">Should varaible be calculated as random value - from start</param>
        /// <param name="autoCalculation">Should variable be calculated automatically from start</param>
        public Int16Variable(String name, Int32 offset, Boolean shouldInsertToHR = true, Boolean shouldInsertToIR = true, Boolean randomGeneration = false, Boolean autoCalculation = false) :
            base(name, offset, shouldInsertToHR, shouldInsertToIR, randomGeneration, autoCalculation)
        {
            this.Data = new Int16[] { 0 };
        }

        private Int16 _randomMax = 0;
        /// <summary>
        /// Random max value
        /// </summary>
        public Int16 RandomMax
        {
            get
            {
                return _randomMax;
            }

            set
            {
                if (value >= RandomMin)
                {
                    _randomMax = value;
                }

                OnPropertyChanged("RandomMax");
            }
        }

        private Int16 _randomMin = 0;
        /// <summary>
        /// Random min value
        /// </summary>
        public Int16 RandomMin
        {
            get
            {
                return _randomMin;
            }

            set
            {
                if (value <= RandomMax)
                {
                    _randomMin = value;
                }

                OnPropertyChanged("RandomMin");
            }
        }

        /// <summary>
        /// Method for generating random value of given range
        /// </summary>
        /// <returns>
        /// Random value from given range
        /// </returns>
        public override object GenerateRandom()
        {
            return RND.Next(RandomMin,RandomMax);
        }

        /// <summary>
        /// Method for converting data to value of variable
        /// </summary>
        /// <param name="Data">Data to convert</param>
        /// <returns>Variable value</returns>
        protected override object ConvertDataToValue(Int16[] Data)
        {
            var firstRegisterBytes = BitConverter.GetBytes(Data[0]);

            var allBytes = new Byte[] { firstRegisterBytes[0], firstRegisterBytes[1] };

            return BitConverter.ToInt16(allBytes, 0);
        }

        /// <summary>
        /// Method for converting value to modbus registers
        /// </summary>
        /// <param name="value">Value to convert</param>
        /// <returns>Modbus registers data</returns>
        protected override Int16[] ConvertValueToData(object value)
        {
            var allBytes = BitConverter.GetBytes(Convert.ToInt16(value));

            return new Int16[] { BitConverter.ToInt16(allBytes, 0) };
        }

        /// <summary>
        /// Method for getting user control associated with variable
        /// </summary>
        /// <returns>
        /// User control associated with variable
        /// </returns>
        public override UserControl GetUserControl()
        {
            var userControlToReturn = new Int16VariableUserControl();
            userControlToReturn.Init(this);
            return userControlToReturn;
        }
    }
}
