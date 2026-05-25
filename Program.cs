using System;
using System.Windows.Forms;
using ESRI.ArcGIS;
using ESRI.ArcGIS.esriSystem;

namespace Route_of_Shanghai
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            // 1. 绑定 ArcGIS 版本 
            RuntimeManager.Bind(ProductCode.Desktop); 

            // 2. 初始化许可 
            IAoInitialize aoInit = new AoInitialize(); 
            esriLicenseProductCode code = esriLicenseProductCode.esriLicenseProductCodeEngine; 

            esriLicenseStatus status = aoInit.Initialize(code); 
            if (status != esriLicenseStatus.esriLicenseCheckedOut) 
            { 
                MessageBox.Show("许可初始化失败！"); 
                return; 
            } 

            // 3. 启动窗口 
            Application.EnableVisualStyles(); 
            Application.SetCompatibleTextRenderingDefault(false); 
            Application.Run(new Form1()); 

            // 4. 释放许可 
            aoInit.Shutdown();
        }
    }
}