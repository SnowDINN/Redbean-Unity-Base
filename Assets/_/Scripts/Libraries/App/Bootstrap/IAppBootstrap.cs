﻿using System;
using System.Threading.Tasks;

namespace Redbean
{
	public interface IAppBootstrap : IDisposable
	{
		/// <summary>
		/// 실행 타입
		/// </summary>
		BootstrapType ExecutionType { get; }
		
		/// <summary>
		/// 실행 순서
		/// </summary>
		int ExecutionOrder { get; }

		/// <summary>
		/// 메모리 해제 순서
		/// </summary>
		int DisposeOrder => (int)ExecutionType + ExecutionOrder;
		
		/// <summary>
		/// 앱 시작 시 실행되는 함수
		/// </summary>
		Task Setup();
	}
}