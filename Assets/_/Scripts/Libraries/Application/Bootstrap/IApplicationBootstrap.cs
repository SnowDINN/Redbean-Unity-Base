﻿using System;
using System.Threading.Tasks;

namespace Redbean
{
	public interface IApplicationBootstrap : IDisposable
	{
		/// <summary>
		/// 실행 순서
		/// </summary>
		int ExecutionOrder { get; }
		
		/// <summary>
		/// 앱 시작 시 실행되는 함수
		/// </summary>
		Task Setup();
	}
}