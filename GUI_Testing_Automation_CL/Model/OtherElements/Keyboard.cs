// Copyright (c) 2017 fit.uet.vnu.edu.vn
// author @duongtd
// created on 9:39 AM 2017/12/14
using System;
using System.Collections.Generic;
using WindowsInput;
using WindowsInput.Native;

namespace GUI_Testing_Automation
{
    /// <summary>
    /// represent for Keyboard
    /// </summary>

    public class Keyboard
    {
        #region define const
        public const string LBUTTON = "LBUTTON";
        public const string RBUTTON = "RBUTTON";
        public const string MBUTTON = "MBUTTON";
        public const string K_CANCEL = "CANCEL";
        public const string K_BACK = "BACK";
        public const string K_TAB = "TAB";
        public const string K_CLEAR = "CLEAR";
        public const string K_ENTER = "ENTER";
        public const string K_SHIFT = "SHIFT";
        public const string K_CTRL = "CTRL";
        public const string K_ALT = "ALT";
        public const string K_PAUSE = "PAUSE";
        public const string K_CAPSLOCK = "CAPSLOCK";
        public const string K_KANA = "KANA";
        public const string K_HANGEUL = "HANGEUL";
        public const string K_HANGUL = "HANGUL";
        public const string K_JUNJA = "JUNJA";
        public const string K_FINAL = "FINAL";
        public const string K_HANJA = "HANJA";
        public const string K_KANJI = "KANJI";
        public const string K_ESCAPE = "ESCAPE";
        public const string K_CONVERT = "CONVERT";
        public const string K_NONCONVERT = "NONCONVERT";
        public const string K_ACCEPT = "ACCEPT";
        public const string K_MODECHANGE = "MODECHANGE";
        public const string K_SPACE = "SPACE";
        public const string K_PAGE_UP = "PAGE_UP";
        public const string K_PAGE_DOWN = "PAGE_DOWN";
        public const string K_END = "END";
        public const string K_HOME = "HOME";
        public const string K_ARROW_LEFT = "LEFT";
        public const string K_ARROW_RIGHT = "RIGHT";
        public const string K_ARROW_DOWN = "DOWN";
        public const string K_ARROW_UP = "UP";
        public const string K_SELECT = "SELECT";
        public const string K_PRINT = "PRINT";
        public const string K_EXECUTE = "EXECUTE";
        public const string K_PRINT_SCREEN = "SNAPSHOT";
        public const string K_INSERT = "INSERT";
        public const string K_DELETE = "DELETE";
        public const string K_HELP = "HELP";
        public const string K_0 = "0";
        public const string K_1 = "1";
        public const string K_2 = "2";
        public const string K_3 = "3";
        public const string K_4 = "4";
        public const string K_5 = "5";
        public const string K_6 = "6";
        public const string K_7 = "7";
        public const string K_8 = "8";
        public const string K_9 = "9";
        public const string K_A = "A";
        public const string K_B = "B";
        public const string K_C = "C";
        public const string K_D = "D";
        public const string K_E = "E";
        public const string K_F = "F";
        public const string K_G = "G";
        public const string K_H = "H";
        public const string K_I = "I";
        public const string K_J = "J";
        public const string K_K = "K";
        public const string K_L = "L";
        public const string K_M = "M";
        public const string K_N = "N";
        public const string K_O = "O";
        public const string K_P = "P";
        public const string K_Q = "Q";
        public const string K_R = "R";
        public const string K_S = "S";
        public const string K_T = "T";
        public const string K_U = "U";
        public const string K_V = "V";
        public const string K_W = "W";
        public const string K_X = "X";
        public const string K_Y = "Y";
        public const string K_Z = "Z";
        public const string K_LWIN = "LWIN";
        public const string K_RWIN = "RWIN";
        public const string K_APPS = "APPS";
        public const string K_SLEEP = "SLEEP";
        public const string K_NUMPAD0 = "NUMPAD0";
        public const string K_NUMPAD1 = "NUMPAD1";
        public const string K_NUMPAD2 = "NUMPAD2";
        public const string K_NUMPAD3 = "NUMPAD3";
        public const string K_NUMPAD4 = "NUMPAD4";
        public const string K_NUMPAD5 = "NUMPAD5";
        public const string K_NUMPAD6 = "NUMPAD6";
        public const string K_NUMPAD7 = "NUMPAD7";
        public const string K_NUMPAD8 = "NUMPAD8";
        public const string K_NUMPAD9 = "NUMPAD9";
        public const string K_MULTIPLY = "MULTIPLY";
        public const string K_ADD = "ADD";
        public const string K_SEPARATOR = "SEPARATOR";
        public const string K_SUBTRACT = "SUBTRACT";
        public const string K_DECIMAL = "DECIMAL";
        public const string K_DIVIDE = "DIVIDE";
        public const string K_F1 = "F1";
        public const string K_F2 = "F2";
        public const string K_F3 = "F3";
        public const string K_F4 = "F4";
        public const string K_F5 = "F5";
        public const string K_F6 = "F6";
        public const string K_F7 = "F7";
        public const string K_F8 = "F8";
        public const string K_F9 = "F9";
        public const string K_F10 = "F10";
        public const string K_F11 = "F11";
        public const string K_F12 = "F12";
        public const string K_F13 = "F13";
        public const string K_F14 = "F14";
        public const string K_F15 = "F15";
        public const string K_F16 = "F16";
        public const string K_F17 = "F17";
        public const string K_F18 = "F18";
        public const string K_F19 = "F19";
        public const string K_F20 = "F20";
        public const string K_F21 = "F21";
        public const string K_F22 = "F22";
        public const string K_F23 = "F23";
        public const string K_F24 = "F24";
        public const string K_NUMLOCK = "NUMLOCK";
        public const string K_SCROLL = "SCROLL";
        public const string K_ZOOM = "ZOOM";
        public const string K_PLAY = "PLAY";
        //
        //     Windows 2000/XP
        public const string K_BROWSER_BACK = "BROWSER_BACK";
        //
        //     Windows 2000/XP
        public const string K_BROWSER_FORWARD = "BROWSER_FORWARD";
        //
        //     Windows 2000/XP
        public const string K_BROWSER_REFRESH = "BROWSER_REFRESH";
        //
        //     Windows 2000/XP
        public const string K_BROWSER_STOP = "BROWSER_STOP";
        //
        //     Windows 2000/XP
        public const string K_BROWSER_SEARCH = "BROWSER_SEARCH";
        //
        //     Windows 2000/XP
        public const string K_BROWSER_FAVORITE = "BROWSER_FAVORITE";
        //
        //     Windows 2000/XP: BROWSER START AND HOME KEY
        public const string K_BROWSER_HOME = "BROWSER_HOME";
        //
        //     Windows 2000/XP
        public const string K_VOLUMN_MUTE = "VOLUMN_MUTE";
        //
        //     Windows 2000/XP
        public const string K_VOLUME_DOWN = "VOLUMN_DOWN";
        //
        //     Windows 2000/XP
        public const string K_VOLUME_UP = "VOLUMN_UP";
        //
        //     Windows 2000/XP
        public const string K_MEDIA_NEXT_TRACK = "MEDIA_NEXT_TRACK";
        //
        //     Windows 2000/XP
        public const string K_MEDIA_PREV_TRACK= "MEDIA_PREV_TRACK";
        //
        //     Windows 2000/XP
        public const string K_MEDIA_STOP = "MEDIA_STOP";
        //
        //     Windows 2000/XP
        public const string K_MEDIA_PLAY_PAUSE = "MEDIA_PLAY_PAUSE";
        //
        //     Windows 2000/XP: START MAIL
        public const string K_LAUNCH_MAIL = "LAUNCH_MAIL";
        //
        // Summary:
        //     Windows 2000/XP: Select Media key
        public const string K_MEDIA_SELECT = "MEDIA_SELECT";
        //
        //     Windows 2000/XP: START APP_1
        public const string K_LAUNCH_APP1 = "LAUNCH_APP1";
        //
        //     Windows 2000/XP: START APP_2
        public const string K_LAUNCH_APP2 = "LAUNCH_APP2";
        //
        // Summary:
        //     Used for miscellaneous characters; it can vary by keyboard. Windows 2000/XP:
        //     For the US standard keyboard, the ';:' key
        public const string K_OEM1 = "OEM1";
        //
        // Summary:
        //     Windows 2000/XP: For any country/region, the '+' key
        public const string K_OEM_PLUS = "OEM_PLUS";
        //
        // Summary:
        //     Windows 2000/XP: For any country/region, the ',' key
        public const string K_OEM_COMMA = "OEM_COMMA";
        //
        // Summary:
        //     Windows 2000/XP: For any country/region, the '-' key
        public const string K_OEM_MINUS = "OEM_MINUS";
        //
        // Summary:
        //     Windows 2000/XP: For any country/region, the '.' key
        public const string K_OEM_PERIOD = "PERIOD";
        //
        // Summary:
        //     Used for miscellaneous characters; it can vary by keyboard. Windows 2000/XP:
        //     For the US standard keyboard, the '/?' key
        public const string K_OEM2 = "OEM2";
        //
        // Summary:
        //     Used for miscellaneous characters; it can vary by keyboard. Windows 2000/XP:
        //     For the US standard keyboard, the '`~' key
        public const string K_OEM3 = "OEM3";
        //
        // Summary:
        //     Used for miscellaneous characters; it can vary by keyboard. Windows 2000/XP:
        //     For the US standard keyboard, the '[{' key
        public const string K_OEM4 = "OEM4";
        //
        // Summary:
        //     Used for miscellaneous characters; it can vary by keyboard. Windows 2000/XP:
        //     For the US standard keyboard, the '\|' key
        public const string K_OEM5 = "OEM5";
        //
        // Summary:
        //     Used for miscellaneous characters; it can vary by keyboard. Windows 2000/XP:
        //     For the US standard keyboard, the ']}' key
        public const string K_OEM6 = "OEM6";
        //
        // Summary:
        //     Used for miscellaneous characters; it can vary by keyboard. Windows 2000/XP:
        //     For the US standard keyboard, the 'single-quote/double-quote' key
        public const string K_OEM7 = "OEM7";
        //
        // Summary:
        //     Used for miscellaneous characters; it can vary by keyboard.
        public const string K_OEM8 = "OEM8";
        //
        // Summary:
        //     Windows 2000/XP: Either the angle bracket key or the backslash key on the RT
        //     102-key keyboard
        public const string K_OEM102 = "OEM102";
        public const string K_ATTN = "ATTN";
        public const string K_CRSEL = "CRSEL";
        public const string K_EXSEL = "EXSEL";
        //
        // Summary:
        //     Erase EOF key
        public const string K_EREOF = "EREOF";
        public const string K_REVERT = "REVERT";

        //K_LSHIFT - Used only as parameters to GetAsyncKeyState() and GetKeyState()
        public const string K_LSHIFT = "LSHIFT";
        //K_RSHIFT - Used only as parameters to GetAsyncKeyState() and GetKeyState()
        public const string K_RSHIFT = "RSHIFT";
        //K_LCTRL - Used only as parameters to GetAsyncKeyState() and GetKeyState()
        public const string K_LCTRL = "LCTRL";
        //K_RCTRL - Used only as parameters to GetAsyncKeyState() and GetKeyState()
        public const string K_RCTRL = "RCTRL";
        //K_LALT - Used only as parameters to GetAsyncKeyState() and GetKeyState()
        public const string K_LALT = "LALT";
        //K_RALT - Used only as parameters to GetAsyncKeyState() and GetKeyState()
        public const string K_RALT = "RALT";
        #endregion define const

        private static Dictionary<string, VirtualKeyCode> mapKeyCode = new Dictionary<string, VirtualKeyCode>();
        private static Dictionary<string, VirtualKeyCode> GetMapKeyCode()
        {
            if (mapKeyCode.Count == 0)
            {
                mapKeyCode.Add(LBUTTON, VirtualKeyCode.LBUTTON);
                mapKeyCode.Add(RBUTTON, VirtualKeyCode.RBUTTON);
                mapKeyCode.Add(K_CANCEL, VirtualKeyCode.CANCEL);
                mapKeyCode.Add(MBUTTON, VirtualKeyCode.MBUTTON);
                mapKeyCode.Add(K_BACK, VirtualKeyCode.BACK);
                mapKeyCode.Add(K_TAB, VirtualKeyCode.TAB);
                mapKeyCode.Add(K_CLEAR, VirtualKeyCode.CLEAR);
                mapKeyCode.Add(K_ENTER, VirtualKeyCode.RETURN);
                mapKeyCode.Add(K_LSHIFT, VirtualKeyCode.LSHIFT);
                mapKeyCode.Add(K_RSHIFT, VirtualKeyCode.RSHIFT);
                mapKeyCode.Add(K_SHIFT, VirtualKeyCode.SHIFT);
                mapKeyCode.Add(K_CTRL, VirtualKeyCode.CONTROL);
                mapKeyCode.Add(K_ALT, VirtualKeyCode.MENU);
                mapKeyCode.Add(K_PAUSE, VirtualKeyCode.PAUSE);
                mapKeyCode.Add(K_CAPSLOCK, VirtualKeyCode.CAPITAL);
                mapKeyCode.Add(K_KANA, VirtualKeyCode.KANA);
                mapKeyCode.Add(K_HANGEUL, VirtualKeyCode.HANGEUL);
                mapKeyCode.Add(K_HANGUL, VirtualKeyCode.HANGUL);
                mapKeyCode.Add(K_JUNJA, VirtualKeyCode.JUNJA);
                mapKeyCode.Add(K_FINAL, VirtualKeyCode.FINAL);
                mapKeyCode.Add(K_HANJA, VirtualKeyCode.HANJA);
                mapKeyCode.Add(K_KANJI, VirtualKeyCode.KANJI);
                mapKeyCode.Add(K_ESCAPE, VirtualKeyCode.ESCAPE);
                mapKeyCode.Add(K_CONVERT, VirtualKeyCode.CONVERT);
                mapKeyCode.Add(K_NONCONVERT, VirtualKeyCode.NONCONVERT);
                mapKeyCode.Add(K_ACCEPT, VirtualKeyCode.ACCEPT);
                mapKeyCode.Add(K_MODECHANGE, VirtualKeyCode.MODECHANGE);
                mapKeyCode.Add(K_SPACE, VirtualKeyCode.SPACE);
                mapKeyCode.Add(K_PAGE_UP, VirtualKeyCode.PRIOR);
                mapKeyCode.Add(K_PAGE_DOWN, VirtualKeyCode.NEXT);
                mapKeyCode.Add(K_END, VirtualKeyCode.END);
                mapKeyCode.Add(K_HOME, VirtualKeyCode.HOME);
                mapKeyCode.Add(K_ARROW_LEFT, VirtualKeyCode.LEFT);
                mapKeyCode.Add(K_ARROW_RIGHT, VirtualKeyCode.RIGHT);
                mapKeyCode.Add(K_ARROW_DOWN, VirtualKeyCode.DOWN);
                mapKeyCode.Add(K_ARROW_UP, VirtualKeyCode.UP);
                mapKeyCode.Add(K_SELECT, VirtualKeyCode.SELECT);
                mapKeyCode.Add(K_PRINT, VirtualKeyCode.PRINT);
                mapKeyCode.Add(K_EXECUTE, VirtualKeyCode.EXECUTE);
                mapKeyCode.Add(K_PRINT_SCREEN, VirtualKeyCode.SNAPSHOT);
                mapKeyCode.Add(K_INSERT, VirtualKeyCode.INSERT);
                mapKeyCode.Add(K_DELETE, VirtualKeyCode.DELETE);
                mapKeyCode.Add(K_HELP, VirtualKeyCode.HELP);
                mapKeyCode.Add(K_0, VirtualKeyCode.VK_0);
                mapKeyCode.Add(K_1, VirtualKeyCode.VK_1);
                mapKeyCode.Add(K_2, VirtualKeyCode.VK_2);
                mapKeyCode.Add(K_3, VirtualKeyCode.VK_3);
                mapKeyCode.Add(K_4, VirtualKeyCode.VK_4);
                mapKeyCode.Add(K_5, VirtualKeyCode.VK_5);
                mapKeyCode.Add(K_6, VirtualKeyCode.VK_6);
                mapKeyCode.Add(K_7, VirtualKeyCode.VK_7);
                mapKeyCode.Add(K_8, VirtualKeyCode.VK_8);
                mapKeyCode.Add(K_9, VirtualKeyCode.VK_9);
                mapKeyCode.Add(K_A, VirtualKeyCode.VK_A);
                mapKeyCode.Add(K_B, VirtualKeyCode.VK_B);
                mapKeyCode.Add(K_C, VirtualKeyCode.VK_C);
                mapKeyCode.Add(K_D, VirtualKeyCode.VK_D);
                mapKeyCode.Add(K_E, VirtualKeyCode.VK_E);
                mapKeyCode.Add(K_F, VirtualKeyCode.VK_F);
                mapKeyCode.Add(K_G, VirtualKeyCode.VK_G);
                mapKeyCode.Add(K_H, VirtualKeyCode.VK_H);
                mapKeyCode.Add(K_J, VirtualKeyCode.VK_J);
                mapKeyCode.Add(K_K, VirtualKeyCode.VK_K);
                mapKeyCode.Add(K_L, VirtualKeyCode.VK_L);
                mapKeyCode.Add(K_M, VirtualKeyCode.VK_M);
                mapKeyCode.Add(K_N, VirtualKeyCode.VK_N);
                mapKeyCode.Add(K_O, VirtualKeyCode.VK_O);
                mapKeyCode.Add(K_P, VirtualKeyCode.VK_P);
                mapKeyCode.Add(K_Q, VirtualKeyCode.VK_Q);
                mapKeyCode.Add(K_R, VirtualKeyCode.VK_R);
                mapKeyCode.Add(K_S, VirtualKeyCode.VK_S);
                mapKeyCode.Add(K_T, VirtualKeyCode.VK_T);
                mapKeyCode.Add(K_U, VirtualKeyCode.VK_U);
                mapKeyCode.Add(K_V, VirtualKeyCode.VK_V);
                mapKeyCode.Add(K_W, VirtualKeyCode.VK_W);
                mapKeyCode.Add(K_X, VirtualKeyCode.VK_X);
                mapKeyCode.Add(K_Y, VirtualKeyCode.VK_Y);
                mapKeyCode.Add(K_Z, VirtualKeyCode.VK_Z);
                mapKeyCode.Add(K_LWIN, VirtualKeyCode.LWIN);
                mapKeyCode.Add(K_RWIN, VirtualKeyCode.RWIN);
                mapKeyCode.Add(K_APPS, VirtualKeyCode.APPS);
                mapKeyCode.Add(K_SLEEP, VirtualKeyCode.SLEEP);
                mapKeyCode.Add(K_NUMPAD0, VirtualKeyCode.NUMPAD0);
                mapKeyCode.Add(K_NUMPAD1, VirtualKeyCode.NUMPAD1);
                mapKeyCode.Add(K_NUMPAD2, VirtualKeyCode.NUMPAD2);
                mapKeyCode.Add(K_NUMPAD3, VirtualKeyCode.NUMPAD3);
                mapKeyCode.Add(K_NUMPAD4, VirtualKeyCode.NUMPAD4);
                mapKeyCode.Add(K_NUMPAD5, VirtualKeyCode.NUMPAD5);
                mapKeyCode.Add(K_NUMPAD6, VirtualKeyCode.NUMPAD6);
                mapKeyCode.Add(K_NUMPAD7, VirtualKeyCode.NUMPAD7);
                mapKeyCode.Add(K_NUMPAD8, VirtualKeyCode.NUMPAD8);
                mapKeyCode.Add(K_NUMPAD9, VirtualKeyCode.NUMPAD9);
                mapKeyCode.Add(K_MULTIPLY, VirtualKeyCode.MULTIPLY);
                mapKeyCode.Add(K_ADD, VirtualKeyCode.ADD);
                mapKeyCode.Add(K_SEPARATOR, VirtualKeyCode.SEPARATOR);
                mapKeyCode.Add(K_DECIMAL, VirtualKeyCode.DECIMAL);
                mapKeyCode.Add(K_DIVIDE, VirtualKeyCode.DIVIDE);
                mapKeyCode.Add(K_F1, VirtualKeyCode.F1);
                mapKeyCode.Add(K_F2, VirtualKeyCode.F2);
                mapKeyCode.Add(K_F3, VirtualKeyCode.F3);
                mapKeyCode.Add(K_F4, VirtualKeyCode.F4);
                mapKeyCode.Add(K_F5, VirtualKeyCode.F5);
                mapKeyCode.Add(K_F6, VirtualKeyCode.F6);
                mapKeyCode.Add(K_F7, VirtualKeyCode.F7);
                mapKeyCode.Add(K_F8, VirtualKeyCode.F8);
                mapKeyCode.Add(K_F9, VirtualKeyCode.F9);
                mapKeyCode.Add(K_F10, VirtualKeyCode.F10);
                mapKeyCode.Add(K_F11, VirtualKeyCode.F11);
                mapKeyCode.Add(K_F12, VirtualKeyCode.F12);
                mapKeyCode.Add(K_F13, VirtualKeyCode.F13);
                mapKeyCode.Add(K_F14, VirtualKeyCode.F14);
                mapKeyCode.Add(K_F15, VirtualKeyCode.F15);
                mapKeyCode.Add(K_F16, VirtualKeyCode.F16);
                mapKeyCode.Add(K_F17, VirtualKeyCode.F17);
                mapKeyCode.Add(K_F18, VirtualKeyCode.F18);
                mapKeyCode.Add(K_F19, VirtualKeyCode.F19);
                mapKeyCode.Add(K_F20, VirtualKeyCode.F20);
                mapKeyCode.Add(K_F21, VirtualKeyCode.F21);
                mapKeyCode.Add(K_F22, VirtualKeyCode.F22);
                mapKeyCode.Add(K_F23, VirtualKeyCode.F23);
                mapKeyCode.Add(K_F24, VirtualKeyCode.F24);
                mapKeyCode.Add(K_NUMLOCK, VirtualKeyCode.NUMLOCK);
                mapKeyCode.Add(K_SCROLL, VirtualKeyCode.SCROLL);
                mapKeyCode.Add(K_LCTRL, VirtualKeyCode.LCONTROL);
                mapKeyCode.Add(K_RCTRL, VirtualKeyCode.RCONTROL);
                mapKeyCode.Add(K_LALT, VirtualKeyCode.LMENU);
                mapKeyCode.Add(K_RALT, VirtualKeyCode.RMENU);
                mapKeyCode.Add(K_ZOOM, VirtualKeyCode.ZOOM);
                mapKeyCode.Add(K_PLAY, VirtualKeyCode.PLAY);
                mapKeyCode.Add(K_BROWSER_BACK, VirtualKeyCode.BROWSER_BACK);
                mapKeyCode.Add(K_BROWSER_FORWARD, VirtualKeyCode.BROWSER_FORWARD);
                mapKeyCode.Add(K_BROWSER_FAVORITE, VirtualKeyCode.BROWSER_FAVORITES);
                mapKeyCode.Add(K_BROWSER_HOME, VirtualKeyCode.BROWSER_HOME);
                mapKeyCode.Add(K_BROWSER_REFRESH, VirtualKeyCode.BROWSER_REFRESH);
                mapKeyCode.Add(K_BROWSER_SEARCH, VirtualKeyCode.BROWSER_SEARCH);
                mapKeyCode.Add(K_BROWSER_STOP, VirtualKeyCode.BROWSER_STOP);
                mapKeyCode.Add(K_VOLUMN_MUTE, VirtualKeyCode.VOLUME_MUTE);
                mapKeyCode.Add(K_VOLUME_UP, VirtualKeyCode.VOLUME_UP);
                mapKeyCode.Add(K_VOLUME_DOWN, VirtualKeyCode.VOLUME_DOWN);
                mapKeyCode.Add(K_MEDIA_NEXT_TRACK, VirtualKeyCode.MEDIA_NEXT_TRACK);
                mapKeyCode.Add(K_MEDIA_PLAY_PAUSE, VirtualKeyCode.MEDIA_PLAY_PAUSE);
                mapKeyCode.Add(K_MEDIA_PREV_TRACK, VirtualKeyCode.MEDIA_PREV_TRACK);
                mapKeyCode.Add(K_MEDIA_SELECT, VirtualKeyCode.LAUNCH_MEDIA_SELECT);
                mapKeyCode.Add(K_MEDIA_STOP, VirtualKeyCode.MEDIA_STOP);
                mapKeyCode.Add(K_LAUNCH_APP1, VirtualKeyCode.LAUNCH_APP1);
                mapKeyCode.Add(K_LAUNCH_APP2, VirtualKeyCode.LAUNCH_APP2);
                mapKeyCode.Add(K_OEM1, VirtualKeyCode.OEM_1);
                mapKeyCode.Add(K_OEM2, VirtualKeyCode.OEM_2);
                mapKeyCode.Add(K_OEM3, VirtualKeyCode.OEM_3);
                mapKeyCode.Add(K_OEM4, VirtualKeyCode.OEM_4);
                mapKeyCode.Add(K_OEM5, VirtualKeyCode.OEM_5);
                mapKeyCode.Add(K_OEM6, VirtualKeyCode.OEM_6);
                mapKeyCode.Add(K_OEM7, VirtualKeyCode.OEM_7);
                mapKeyCode.Add(K_OEM8, VirtualKeyCode.OEM_8);
                mapKeyCode.Add(K_OEM102, VirtualKeyCode.OEM_102);
                mapKeyCode.Add(K_OEM_COMMA, VirtualKeyCode.OEM_COMMA);
                mapKeyCode.Add(K_OEM_MINUS, VirtualKeyCode.OEM_MINUS);
                mapKeyCode.Add(K_OEM_PERIOD, VirtualKeyCode.OEM_PERIOD);
                mapKeyCode.Add(K_OEM_PLUS, VirtualKeyCode.OEM_PLUS);
                mapKeyCode.Add(K_ATTN, VirtualKeyCode.ATTN);
                mapKeyCode.Add(K_CRSEL, VirtualKeyCode.CRSEL);
                mapKeyCode.Add(K_EXSEL, VirtualKeyCode.EXSEL);
                mapKeyCode.Add(K_EREOF, VirtualKeyCode.EREOF);
            }
            return mapKeyCode;
        }

        /// <summary>
        /// @author=Duongtd
        /// I temporarily implement a method here
        /// see Class1 in ProjectGenTemplate for more details
        /// </summary>
        /// <param name="key">you should define all Special Key, not use string like this</param>  

        public static bool SendSingleKey(string key)
        {
            Dictionary<string, VirtualKeyCode> mapKeyCode = GetMapKeyCode();
            if (!mapKeyCode.ContainsKey(key)) return true;

            InputSimulator inputSimulator = new InputSimulator();
            inputSimulator.Keyboard.KeyPress(mapKeyCode[key]);
            return true;

        }

        public static bool SendCombinedKeys(params string[] keys)
        {
            return SendCombinedKeys(new List<string>(keys));
        }
       
        /// <summary>
        /// 
        /// </summary>
        /// <param name="listKey">input a list of group key</param>
        /// <returns></returns>
        public static bool SendCombinedKeys(List<string> listKey)
        {
            //check input
            if (listKey == null || listKey.Count == 0) return true;

            Dictionary<string, VirtualKeyCode> mapKeyCode = GetMapKeyCode();
            InputSimulator inputSimulator = new InputSimulator();

            //down all keys 
            foreach (string key in listKey)
            {
                if (mapKeyCode.ContainsKey(key))
                {
                    inputSimulator.Keyboard.KeyDown(mapKeyCode[key]);
                }                
            }

            //up all keys
            foreach (string key in listKey)
            {
                if (mapKeyCode.ContainsKey(key))
                {
                    inputSimulator.Keyboard.KeyUp(mapKeyCode[key]);
                }
            }
            return true;
        }

        ///// <summary>
        ///// <params name="listSingleKeys">Input with a list of string for keyboard </params>       
        ///// </summary>        
        //public static bool SendListSingleKey(List<string> listSingleKeys)
        //{
        //    //check input
        //    if (listSingleKeys == null || listSingleKeys.Count == 0)
        //    {
        //        return true;
        //    }

        //    Dictionary<string, VirtualKeyCode> mapKeyCode = GetMapKeyCode();           
        //    InputSimulator inputSimulator = new InputSimulator();
        //    foreach (string key in listSingleKeys)
        //    {
        //        if (mapKeyCode.ContainsKey(key))
        //        {
        //            inputSimulator.Keyboard.KeyPress(mapKeyCode[key]);
        //        }                
        //    }           

        //    return true;
        //}
    }
}
