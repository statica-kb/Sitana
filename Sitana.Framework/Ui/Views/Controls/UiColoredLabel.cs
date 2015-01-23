﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sitana.Framework.Cs;
using Sitana.Framework.Ui.DefinitionFiles;
using Sitana.Framework.Xml;
using Sitana.Framework.Graphics;
using Sitana.Framework.Ui.Views.Parameters;
using Sitana.Framework.Content;
using Microsoft.Xna.Framework;

namespace Sitana.Framework.Ui.Views
{
    public class UiColoredLabel: UiLabel
    {
        public new static void Parse(XNode node, DefinitionFile file)
        {
            UiLabel.Parse(node, file);

            var parser = new DefinitionParser(node);
            file["Colors"] = parser.ParseColor("Colors");
        }

        public Color[] Colors { get; private set; }

        protected override void Init(object controller, object binding, DefinitionFile definition)
        {
            base.Init(controller, binding, definition);

            DefinitionFileWithStyle file = new DefinitionFileWithStyle(definition, typeof(UiColoredLabel));
            Colors = DefinitionResolver.Get<Color[]>(Controller, Binding, file["Colors"], null);
        }

        protected override void Draw(ref UiViewDrawParameters parameters)
        {
            if (Colors == null)
            {
                base.Draw(ref parameters);
                return;
            }

            float opacity = parameters.Opacity;

            if (opacity == 0 || TextColor.Value.A == 0)
            {
                return;
            }

            if (_fontFace == null)
            {
                _fontFace = FontManager.Instance.FindFont(FontName);
            }

            float scale;
            UniversalFont font = _fontFace.Find(FontSize, out scale);

            if (font.SitanaFont == null)
            {
                throw new Exception("Only Sitana fonts support UiColoredLabel.");
            }

            lock (Text)
            {
                lock (Colors)
                {
                    parameters.DrawBatch.DrawText(font, Text.StringBuilder, ScreenBounds, TextAlign, Colors, opacity, (float)FontSpacing / 1000.0f, (float)LineHeight / 100.0f, scale);
                }
            }
        }
    }
}
