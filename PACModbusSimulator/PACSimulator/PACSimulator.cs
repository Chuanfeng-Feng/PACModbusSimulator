﻿using ModbusSimulator;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Xml.Linq;

namespace PACModbusSimulator
{
    public class PACSimulator : INotifyPropertyChanged
    {
        /// <summary>
        /// Class representing simulator
        /// </summary>
        public PACSimulator()
        {

        }

        /// <summary>
        /// XML settings file name
        /// </summary>
        public const String xmlFileName = "Settings.xml";

        /// <summary>
        /// String representing PAC3200 meter
        /// </summary>
        public const String pac3200String = "PAC3200";

        /// <summary>
        /// String representing PAC3220 meter
        /// </summary>
        public const String pac3220String = "PAC3220";

        /// <summary>
        /// String representing PAC3100 meter
        /// </summary>
        public const String pac3100String = "PAC3100";

        /// <summary>
        /// String representing PAC3120 meter
        /// </summary>
        public const String pac3120String = "PAC3120";

        /// <summary>
        /// String representing PAC4200 meter
        /// </summary>
        public const String pac4200String = "PAC4200";

        /// <summary>
        /// Method for initializing PACSimulator
        /// </summary>
        public void Init()
        {
            this.ReadXMLFile();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

        private ObservableCollection<ModbusMeterBase> _allMBDevices = new ObservableCollection<ModbusMeterBase>();
        /// <summary>
        /// All meters associated with Simulator
        /// </summary>
        public ObservableCollection<ModbusMeterBase> AllMBDevices
        {
            get
            {
                return _allMBDevices;
            }

            private set
            {
                this._allMBDevices = value;
                OnPropertyChanged("AllMBDevices");
            }
        }

        private ModbusMeterBase _currentDevice;
        /// <summary>
        /// Current meter selected by ListBox
        /// </summary>
        public ModbusMeterBase CurrentDevice
        {
            get
            {
                return _currentDevice;
            }

            set
            {
                this._currentDevice = value;
                OnPropertyChanged("CurrentDevice");
            }
        }

        /// <summary>
        /// Method for creating new PAC3200 meter and adding it to AllMeters
        /// </summary>
        /// <param name="name">Name of device</param>
        /// <param name="portNumber">Port number</param>
        /// <param name="unitId">Unit id</param>
        /// <param name="nominalCurrent">Nominal current</param>
        /// <param name="nominialPowerFactor">Nominal power factor</param>
        public void CreatePAC3200Meter(string name, int portNumber, byte unitId, Single nominalCurrent, Single nominialPowerFactor)
        {
            AddDevice(new PAC3200(this,name,portNumber, unitId, nominalCurrent ,nominialPowerFactor));
        }

        /// <summary>
        /// Method for creating new PAC3220 meter and adding it to AllMeters
        /// </summary>
        /// <param name="name">Name of device</param>
        /// <param name="portNumber">Port number</param>
        /// <param name="unitId">Unit id</param>
        /// <param name="nominalCurrent">Nominal current</param>
        /// <param name="nominialPowerFactor">Nominal power factor</param>
        public void CreatePAC3220Meter(string name, int portNumber, byte unitId, Single nominalCurrent, Single nominialPowerFactor)
        {
            AddDevice(new PAC3220(this, name, portNumber, unitId, nominalCurrent, nominialPowerFactor));
        }

        /// <summary>
        /// Method for creating new PAC3100 meter and adding it to AllMeters
        /// </summary>
        /// <param name="name">Name of device</param>
        /// <param name="portNumber">Port number</param>
        /// <param name="unitId">Unit id</param>
        /// <param name="nominalCurrent">Nominal current</param>
        /// <param name="nominialPowerFactor">Nominal power factor</param>
        public void CreatePAC3100Meter(string name, int portNumber, byte unitId, Single nominalCurrent, Single nominialPowerFactor)
        {
            AddDevice(new PAC3100(this, name, portNumber, unitId, nominalCurrent, nominialPowerFactor));
        }

        /// <summary>
        /// Method for creating new PAC3120 meter and adding it to AllMeters
        /// </summary>
        /// <param name="name">Name of device</param>
        /// <param name="portNumber">Port number</param>
        /// <param name="unitId">Unit id</param>
        /// <param name="nominalCurrent">Nominal current</param>
        /// <param name="nominialPowerFactor">Nominal power factor</param>
        public void CreatePAC3120Meter(string name, int portNumber, byte unitId, Single nominalCurrent, Single nominialPowerFactor)
        {
            AddDevice(new PAC3120(this, name, portNumber, unitId, nominalCurrent, nominialPowerFactor));
        }

        /// <summary>
        /// Method for creating new PAC4200 meter and adding it to AllMeters
        /// </summary>
        /// <param name="name">Name of device</param>
        /// <param name="portNumber">Port number</param>
        /// <param name="unitId">Unit id</param>
        /// <param name="nominalCurrent">Nominal current</param>
        /// <param name="nominialPowerFactor">Nominal power factor</param>
        public void CreatePAC4200Meter(string name, int portNumber, byte unitId, Single nominalCurrent, Single nominialPowerFactor)
        {
            AddDevice(new PAC4200(this, name, portNumber, unitId, nominalCurrent, nominialPowerFactor));
        }

        /// <summary>
        /// Method for adding new meter to AllMeters
        /// </summary>
        /// <param name="device">new meter to be added</param>
        public void AddDevice(ModbusMeterBase device)
        {
            AllMBDevices.Add(device);
            device.MBDeviceSettingsChanged += OnDeviceSettingsChanged;
            SaveToXMLFile();
        }

        /// <summary>
        /// Method invoked on one of meters settings change
        /// </summary>
        private void OnDeviceSettingsChanged()
        {
            SaveToXMLFile();
        }

        /// <summary>
        /// Method for saving all settings to XML file
        /// </summary>
        private void SaveToXMLFile()
        {
            try
            {
                XElement rootElement = GetXMLElement();
                if (rootElement != null)
                {
                    XDocument xDocument = new XDocument(rootElement);
                    xDocument.Save(xmlFileName);
                }
            }
            catch (Exception error)
            {
                MessageBox.Show("Error during saving XML file", error.Message, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Method for reading all data from XML file and setting Simulator according to XML file content
        /// </summary>
        private void ReadXMLFile()
        {
            try
            {
                if (File.Exists(xmlFileName))
                {
                    XDocument doc = XDocument.Load(xmlFileName);

                    this.SetFromXML(doc.Root);
                }
            }
            catch (Exception error)
            {
                MessageBox.Show("Error during reading XML file", error.Message, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Method for removing meter
        /// </summary>
        /// <param name="device">
        /// Meter to remove
        /// </param>
        public void RemoveDevice(ModbusMeterBase device)
        {
            //Stopping meter if started
            if(device.IsRunning)
            {
                device.Stop();
            }

            AllMBDevices.Remove(device);
            device.MBDeviceSettingsChanged -= OnDeviceSettingsChanged;
            SaveToXMLFile();
        }

        /// <summary>
        /// Method for checking if port number of new meter is uniq
        /// </summary>
        /// <param name="portNumber">portNumber to check</param>
        /// <param name="portNumber">unitId to check</param>
        /// <returns>is portNumber uniq</returns>
        public Boolean CheckNewPortNumberAndUnitId(int portNumber, byte unitId)
        {
            if (portNumber <= 0 || unitId <= 0) return false;

            //There should be no device with the same combination - unitId/portNumber
            return ModbusFactory.GetDevice(portNumber, unitId) == null;
        }

        /// <summary>
        /// Method for showing meter window
        /// </summary>
        /// <param name="meter"></param>
        public void ShowPacWindow(ModbusMeterBase meter)
        {
            PACWindow window = new PACWindow();
            window.Init(meter);
            window.Show();
        }

        /// <summary>
        /// Method for starting meter
        /// </summary>
        /// <param name="meter">
        /// Meter to start
        /// </param>
        public void StartMeter(ModbusMeterBase meter)
        {
                meter.Start();
        }

        /// <summary>
        /// Method for stopping meter
        /// </summary>
        /// <param name="meter"></param>
        public void StopMeter(ModbusMeterBase meter)
        {
            meter.Stop();
        }

        /// <summary>
        /// Method for setting simulator according to XML content
        /// </summary>
        /// <param name="mainElement">XML element</param>
        public void SetFromXML(XElement mainElement)
        {
            if (mainElement.Name != "PACSimulator")
                throw new Exception("Invalid name of XML file");

            //Clearing all possible selections and seleted elements
            this.AllMBDevices.Clear();
            this.CurrentDevice = null;

            foreach(var element in mainElement.Elements())
            {
                if(element.Name == PAC3200.TypeString)
                {
                    var newPAC3200 = new PAC3200(this,element);

                    AddDevice(newPAC3200);
                }
                else if (element.Name == PAC4200.TypeString)
                {
                    var newPAC4200 = new PAC4200(this,element);

                    AddDevice(newPAC4200);
                }
                else if (element.Name == PAC3220.TypeString)
                {
                    var newPAC3220 = new PAC3220(this,element);

                    AddDevice(newPAC3220);
                }
                else if (element.Name == PAC3100.TypeString)
                {
                    var newPAC3100 = new PAC3100(this, element);

                    AddDevice(newPAC3100);
                }
                else if (element.Name == PAC3120.TypeString)
                {
                    var newPAC3120 = new PAC3120(this, element);

                    AddDevice(newPAC3120);
                }
            }
        }

        /// <summary>
        /// Method for generating content of XML File, storing all settings of simulator
        /// </summary>
        /// <returns>
        /// XML content for storing all settings of simulator
        /// </returns>
        public XElement GetXMLElement()
        {
            XElement mainElement = new XElement("PACSimulator");

            foreach(var meter in AllMBDevices)
            {
                mainElement.Add(meter.GetXMLElement());
            }

            return mainElement;
        }

    }

}
