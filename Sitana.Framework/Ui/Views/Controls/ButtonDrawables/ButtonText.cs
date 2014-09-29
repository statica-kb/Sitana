﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sitana.Framework.Ui.DefinitionFiles;
using Sitana.Framework.Essentials.Ui.DefinitionFiles;
using Sitana.Framework.Xml;
using Sitana.Framework.Ui.Controllers;
using Microsoft.Xna.Framework;
using Sitana.Framework.Graphics;
using Sitana.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Sitana.Framework.Cs;

namespace Sitana.Framework.Ui.Views.ButtonDrawables
{
    public class Text : ButtonDrawable, IDefinitionClass
    {
        public static void Parse(XNode node, DefinitionFile file)
        {
            var parser = new DefinitionParser(node);

            file["ColorPushed"] = parser.ParseColor("ColorPushed");
            file["ColorReleased"] = parser.ParseColor("ColorReleased");
            file["ColorDisabled"] = parser.ParseColor("ColorDisabled");

            file["Font"] = parser.Value("Font");
            file["FontSize"] = parser.ParseInt("FontSize");
            file["TextAlign"] = parser.ParseEnum<TextAlign>("TextAlign", TextAlign.Center | TextAlign.Middle);
            file["Padding"] = parser.ParseInt("Padding");
        }

        protected ColorWrapper _colorPushed;
        protected ColorWrapper _colorReleased;
        protected ColorWrapper _colorDisabled;

        protected string _font;
        protected int _fontSize;
        protected TextAlign _textAlign;
        protected int _padding;

        void IDefinitionClass.Init(UiController controller, object binding, DefinitionFile file)
        {
            Init(controller, binding, file);
        }

        protected virtual void Init(UiController controller, object binding, DefinitionFile file)
        {
            _colorDisabled = DefinitionResolver.GetColorWrapper(controller, binding, file["ColorDisabled"]);
            _colorReleased = DefinitionResolver.GetColorWrapper(controller, binding, file["ColorReleased"]);
            _colorPushed = DefinitionResolver.GetColorWrapper(controller, binding, file["ColorPushed"]);
            _padding = DefinitionResolver.Get<int>(controller, binding, file["Padding"]);

            _font = DefinitionResolver.GetString(controller, binding, file["Font"]);
            _fontSize = DefinitionResolver.Get<int>(controller, binding, file["FontSize"]);
            _textAlign = DefinitionResolver.Get<TextAlign>(controller, binding, file["TextAlign"]);
        }

        public override void Draw(AdvancedDrawBatch drawBatch, Rectangle target, float opacity, UiButton.State state, object argument)
        {
            SharedString str = (SharedString)argument;
            SpriteFont font = FontManager.Instance.FindFont(_font, _fontSize);

            Color color = ColorFromState(state) * opacity;

            Rectangle rect = target;
            rect.Inflate(-_padding, -_padding);

            drawBatch.Font = font;
            drawBatch.DrawText(str, rect, _textAlign, color);
        }

        protected Color ColorFromState(UiButton.State state)
        {
            Color color = Color.Transparent;

            switch (state)
            {
                case UiButton.State.Disabled:
                    color = _colorDisabled.Value;
                    break;

                case UiButton.State.Pushed:
                    color = _colorPushed.Value;
                    break;

                case UiButton.State.Released:
                    color = _colorReleased.Value;
                    break;
            }

            return color;
        }
    }
}