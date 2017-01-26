﻿/******************************************************************************* 
  * Copyright 2015 Esri 
  *  
  *  Licensed under the Apache License, Version 2.0 (the "License"); 
  *  you may not use this file except in compliance with the License. 
  *  You may obtain a copy of the License at 
  *  
  *  http://www.apache.org/licenses/LICENSE-2.0 
  *   
  *   Unless required by applicable law or agreed to in writing, software 
  *   distributed under the License is distributed on an "AS IS" BASIS, 
  *   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. 
  *   See the License for the specific language governing permissions and 
  *   limitations under the License. 
  ******************************************************************************/ 

using System;
using System.Windows;
using System.Windows.Controls;
using ArcGIS.Core.Geometry;
using ArcGIS.Desktop.Framework.Threading.Tasks;

namespace CoordinateSystemAddin.UI {
    /// <summary>
    /// Interaction logic for CoordSysDialog.xaml
    /// </summary>
    public partial class CoordSysDialog : Window {

        private CoordSysPickerViewModel _vm = new CoordSysPickerViewModel();
        private SpatialReference _sr = null;
        //private bool _cancelled = false;

        public CoordSysDialog() {
            InitializeComponent();
            this.DataContext = _vm;
            this.CoordinatePicker.DataContext = _vm;
        }
        /// <summary>
        /// The selected Spatial Reference based on the picker selection
        /// </summary>
        public SpatialReference SpatialReference
        {
            get
            {
                return _sr;
            }
        }

        private async void Close_OnClick(object sender, RoutedEventArgs e) {

            if (((Button)sender).Name == "OK" &&_vm.SelectedCoordSystemInfo != null) {
                //assign the Spatial Reference
                await QueuedTask.Run(() => {
                    try {
                        _sr = SpatialReferenceBuilder.CreateSpatialReference(_vm.SelectedCoordSystemInfo.WKID);
                    }
                    catch (Exception ex) {
                        System.Diagnostics.Debug.WriteLine(ex.ToString());
                    }
                });
            }
            Close();
        }
    }
}
