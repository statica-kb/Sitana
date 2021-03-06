﻿using System;
using Sitana.Framework.Content;
using Sitana.Framework.Ui.Core;
using Microsoft.Xna.Framework;
using Sitana.Framework.Ui.Views;
using Sitana.Framework;
using Sitana.Framework.Input.TouchPad;
using Sitana.Framework.Ui.Controllers;
using Microsoft.Xna.Framework.Graphics;
using System.Text;
using Sitana.Framework.Ui.DefinitionFiles;
using Sitana.Framework.Ui.Views.ButtonDrawables;
using System.Threading;
using Sitana.Framework.Diagnostics;

namespace SampleApp
{
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            using (var main = new AppMain())
            {
                ContentLoader.Init(main.Services, "SampleApp");

                UiUnit.Unit = 1;
                UiUnit.FontUnit = 1;
                UiUnit.FontScaling = UiUnit.ScalingMode.Floating;

                StylesManager.Instance.LoadStyles("Ui/AppStyles", true);
                main.LoadView("Ui/MainView");

                main.Window.AllowUserResizing = true;
                main.Window.ClientSizeChanged += Window_ClientSizeChanged;

                main.Graphics.IsFullScreen = false;

                main.IsMouseVisible = true;
                main.ContentLoading += MainController.OnLoadContent;
                main.ViewLoaded += (a) => a.ResizeToView();

                main.Run();
            }
        }

        static void Window_ClientSizeChanged(object sender, EventArgs e)
        {
            UiTask.BeginInvoke(() =>
            {
                double unit = Math.Min((double)AppMain.Current.GraphicsDevice.Viewport.Width / 640.0,
                                        (double)AppMain.Current.GraphicsDevice.Viewport.Height / 480.0);

                unit = Math.Min(1, unit);

                UiUnit.FontUnit = UiUnit.Unit = unit;
            });
        }

    }
}
