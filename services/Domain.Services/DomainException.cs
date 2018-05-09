﻿using System;
using Domain.Abstractions;

namespace Domain.Services
{
	public class DomainException : Exception
	{
		public DomainException(string message)
			: base(message)
		{ }

		public DomainException(string message, Exception inner)
			: base(message, inner)
		{ }
	}
}
