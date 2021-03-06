﻿#region License
/*
 * TestSocketIO.cs
 *
 * The MIT License
 *
 * Copyright (c) 2014 Fabio Panettieri
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 */
#endregion

using System.Collections;
using UnityEngine;
using SocketIO;
using System.Text.RegularExpressions;
using System.Linq;
using System.Collections.Generic;
using System;



public class TestSocketIO : MonoBehaviour
{
	private SocketIOComponent socket;
    GameObject GameControllerObject;
    GameController gameController; 

	public void Start() 
	{
        GameControllerObject = GameObject.FindGameObjectWithTag("GameController");
        gameController = GameControllerObject.GetComponent<GameController>();

        GameObject go = GameObject.Find("SocketIO");
		socket = go.GetComponent<SocketIOComponent>();

		socket.On("open", TestOpen);
		socket.On("message",OnMessage);
		socket.On("welcome",OnWelcome);
		socket.On("error", TestError);
		socket.On("close", TestClose);

	}
	
	public void OnMessage(SocketIOEvent e)
	{
		Debug.Log("[SocketIO] OnMessage : " + e.name + " " + e.data);
        Regex rx = new Regex(@"([^{.datas.:])([\d])+");
        MatchCollection matches = rx.Matches(e.data.ToString());
        foreach (Match match in matches)
        {
            char[] charsToTrim = { '\"' };
            string complete = match.ToString().Trim(charsToTrim);
            Debug.Log(Int32.Parse(complete));
            gameController.lightLevel = Int32.Parse(complete);

        }

        // Debug.Log("thing" + e.data.ToString());
    }
	
	public void OnWelcome(SocketIOEvent e)
	{
		Debug.Log("[SocketIO] On Welcome: " + e.name + " " + e.data);
	}
	
	// Added a onclick for a button event to send something
	public void OnClick(){
		Debug.Log("OnClick >>>");
		socket.Emit("message");
	}

	

	public void TestOpen(SocketIOEvent e)
	{
		Debug.Log("[SocketIO] Open received: " + e.name + " " + e.data);
	}
	
	
	
	public void TestError(SocketIOEvent e)
	{
		Debug.Log("[SocketIO] Error received: " + e.name + " " + e.data);
	}
	
	public void TestClose(SocketIOEvent e)
	{	
		Debug.Log("[SocketIO] Close received: " + e.name + " " + e.data);
	}

}