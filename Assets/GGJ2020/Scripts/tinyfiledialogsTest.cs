/*_________
 /         \ tinyfiledialogsTest.cs v3.4.3 [Dec 8, 2019] zlib licence
 |tiny file| C# bindings created [2015]
 | dialogs | Copyright (c) 2014 - 2018 Guillaume Vareille http://ysengrin.com
 \____  ___/ http://tinyfiledialogs.sourceforge.net
      \|     git clone http://git.code.sf.net/p/tinyfiledialogs/code tinyfd
         ____________________________________________
	    |                                            |
	    |   email: tinyfiledialogs at ysengrin.com   |
	    |____________________________________________|

Please upvote my stackoverflow answer https://stackoverflow.com/a/47651444
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

class tinyfd
{
    // cross platform utf8
#if UNITY_IPHONE
    [DllImport ("__Internal", CallingConvention = CallingConvention.Cdecl)]
#else
    [DllImport("tinyfiledialogs64.dll", CallingConvention = CallingConvention.Cdecl)]
#endif
    public static extern void tinyfd_beep();
#if UNITY_IPHONE
    [DllImport ("__Internal", CallingConvention = CallingConvention.Cdecl)]
#else
    [DllImport("tinyfiledialogs64.dll", CallingConvention = CallingConvention.Cdecl)]
#endif
    public static extern int tinyfd_notifyPopup(string aTitle, string aMessage, string aIconType);
#if UNITY_IPHONE
    [DllImport ("__Internal", CallingConvention = CallingConvention.Cdecl)]
#else
    [DllImport("tinyfiledialogs64.dll", CallingConvention = CallingConvention.Cdecl)]
#endif
    public static extern int tinyfd_messageBox(string aTitle, string aMessage, string aDialogTyle, string aIconType, int aDefaultButton);
#if UNITY_IPHONE
    [DllImport ("__Internal", CallingConvention = CallingConvention.Cdecl)]
#else
    [DllImport("tinyfiledialogs64.dll", CallingConvention = CallingConvention.Cdecl)]
#endif
    public static extern IntPtr tinyfd_inputBox(string aTitle, string aMessage, string aDefaultInput);
#if UNITY_IPHONE
    [DllImport ("__Internal", CallingConvention = CallingConvention.Cdecl)]
#else
    [DllImport("tinyfiledialogs64.dll", CallingConvention = CallingConvention.Cdecl)]
#endif
    public static extern IntPtr tinyfd_saveFileDialog(string aTitle, string aDefaultPathAndFile, int aNumOfFilterPatterns, string[] aFilterPatterns, string aSingleFilterDescription);
#if UNITY_IPHONE
    [DllImport ("__Internal", CallingConvention = CallingConvention.Cdecl)]
#else
    [DllImport("tinyfiledialogs64.dll", CallingConvention = CallingConvention.Cdecl)]
#endif
    public static extern IntPtr tinyfd_openFileDialog(string aTitle, string aDefaultPathAndFile, int aNumOfFilterPatterns, string[] aFilterPatterns, string aSingleFilterDescription, int aAllowMultipleSelects);
#if UNITY_IPHONE
    [DllImport ("__Internal", CallingConvention = CallingConvention.Cdecl)]
#else
    [DllImport("tinyfiledialogs64.dll", CallingConvention = CallingConvention.Cdecl)]
#endif
    public static extern IntPtr tinyfd_selectFolderDialog(string aTitle, string aDefaultPathAndFile);
#if UNITY_IPHONE
    [DllImport ("__Internal", CallingConvention = CallingConvention.Cdecl)]
#else
    [DllImport("tinyfiledialogs64.dll", CallingConvention = CallingConvention.Cdecl)]
#endif
    public static extern IntPtr tinyfd_colorChooser(string aTitle, string aDefaultHexRGB, byte[] aDefaultRGB, byte[] aoResultRGB);
}
