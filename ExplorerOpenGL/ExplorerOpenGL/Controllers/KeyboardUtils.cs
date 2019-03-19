﻿using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExplorerOpenGL.Controllers
{
    public class KeyboardUtils
    {
        KeyboardState currentKeyboardState;
        KeyboardState previousKeyboardState;

        public delegate void KeyPressedEventHandler(Keys[] keys);
        public event KeyPressedEventHandler KeyPressed;

        public delegate void KeyReleasedEventHandler(Keys[] keys);
        public event KeyReleasedEventHandler KeyRealeased;


        public KeyboardUtils()
        {

        }

        public bool IsContaining(Keys[] keys, Keys seekingKey)
        {
            int index = Array.IndexOf(keys, seekingKey);
            if (index > -1)
                return true;
            return false; 
        }

        public void Update()
        {
            previousKeyboardState = currentKeyboardState;
            currentKeyboardState = Keyboard.GetState();

            if (previousKeyboardState == currentKeyboardState)
                return;

            Keys[] currentPressedKeys = currentKeyboardState.GetPressedKeys();
            Keys[] previousPressedKeys = previousKeyboardState.GetPressedKeys();

            Keys[] NewKeys = GetPressedKey(currentPressedKeys, previousPressedKeys);
            Keys[] LostKeys = GetReleasedKey(currentPressedKeys, previousPressedKeys);

            if (NewKeys.Length > 0) 
            {
                OnKeyPressed(NewKeys); 
            }
            if (LostKeys.Length > 0) 
            {
                OnKeyRelease(LostKeys); 
            }

        }

        private Keys[] GetReleasedKey(Keys[] currentPressedKeys, Keys[] previousPressedKeys)
        {
            List<Keys> KeyReleased = new List<Keys>();

            for (int i = 0; i < currentPressedKeys.Length; i++)
            {
                bool Released = true;
                for (int j = 0; j < previousPressedKeys.Length; j++)
                {
                    if (previousPressedKeys[j] == currentPressedKeys[i])
                    {
                        Released = false;
                        break;
                    }
                }
                if (Released)
                {
                    KeyReleased.Add(currentPressedKeys[i]);
                }
            }
            return KeyReleased.ToArray(); 
        }

        private Keys[] GetPressedKey(Keys[] currentPressedKeys, Keys[] previousPressedKeys)
        {

            List<Keys> KeyPressed = new List<Keys>();

            for (int i = 0; i < currentPressedKeys.Length; i++)
            {
                bool Pressed = true;
                for (int j = 0; j < previousPressedKeys.Length; j++)
                {
                    if (previousPressedKeys[j] == currentPressedKeys[i])
                    {
                        Pressed = false;
                        break;
                    }
                }
                if (Pressed)
                {
                    KeyPressed.Add(currentPressedKeys[i]);
                }
            }
            return KeyPressed.ToArray();
        }

        protected virtual void OnKeyRelease(Keys[] keys)
        {
            KeyRealeased?.Invoke(keys);
        }

        protected virtual void OnKeyPressed(Keys[] keys)
        {
            KeyPressed?.Invoke(keys); 
        }
    }
}
