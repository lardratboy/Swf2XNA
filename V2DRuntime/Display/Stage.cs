﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using DDW.V2D;

namespace DDW.Display
{
    public class Stage : DisplayObjectContainer
    {
        protected List<Screen> screens = new List<Screen>();
        protected int curScreenIndex = 0;
        protected Screen curScreen;
        protected Screen prevScreen;

        public float MillisecondsPerFrame = 1000f / 12f;

        protected Stage()
        {
            stage = this;
        }

        public void AddScreen(Screen scr)
        {
            screens.Add(scr);
        }
        public void RemoveScreen(Screen scr)
        {
            screens.Remove(scr);
        }

		public Screen GetCurrentScreen()
		{
			return curScreen;
		}
        public void SetScreen(Screen scr)
        {
            if (scr != null)
            {
                if (curScreen != null)
                {
                    prevScreen = curScreen;
                    // can remove on fade etc here
                    curScreen.isActive = false;
                    //stage.RemoveChild(curScreen);
                }

                curScreenIndex = screens.IndexOf(scr);
                curScreen = scr;
            }
        }
        public void SetScreen(string screenName)
        {
            Screen scr = screens.Find(sc => sc.InstanceName == screenName);
            SetScreen(scr);
        }
        public void SetScreen(int index)
        {
            if (index >= screens.Count)
            {
                index = 0;
            }
            else if (index < 0)
            {
                index = screens.Count - 1;
            }

            SetScreen(screens[index]);
        }
        public void NextScreen()
        {
            SetScreen(curScreenIndex + 1);
        }
        public void PreviousScreen()
        {
            SetScreen(curScreenIndex - 1);
        }

        internal virtual void ObjectAddedToStage(DisplayObject o)
        {
			o.AddedToStage(EventArgs.Empty);
        }
        internal virtual void ObjectRemovedFromStage(DisplayObject o)
        {
        }
        public override void Update(GameTime gameTime)
        {
            //base.Update(gameTime); // dont update stage itself as it is fixed pos
			if (V2DGame.instance.IsActive)
			{
				foreach (DisplayObject d in children)
				{
					d.Update(gameTime);
				}
			}

            if (prevScreen != null && children.Contains(prevScreen) && !prevScreen.isActive)
            {
                this.RemoveChild(prevScreen);
                prevScreen = null;
            }

            if (!children.Contains(curScreen))
            {
                this.AddChild(curScreen);
                if (curScreen is V2DScreen)
                {
                    V2DGame.instance.SetSize(((V2DScreen)curScreen).v2dWorld.Width, ((V2DScreen)curScreen).v2dWorld.Height);
                }
            }
		}
        public override void Draw(SpriteBatch batch)
        {
            DepthCounter = 1;

            batch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.None);
            base.Draw(batch);
            batch.End();

        }
    }
}





