﻿/*
Copyright (c) 2013, Darren Horrocks
All rights reserved.

Redistribution and use in source and binary forms, with or without modification,
are permitted provided that the following conditions are met:

* Redistributions of source code must retain the above copyright notice, this
  list of conditions and the following disclaimer.

* Redistributions in binary form must reproduce the above copyright notice, this
  list of conditions and the following disclaimer in the documentation and/or
  other materials provided with the distribution.

* Neither the name of Darren Horrocks nor the names of its
  contributors may be used to endorse or promote products derived from
  this software without specific prior written permission.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR
ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
(INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON
ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
(INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE. 
*/

using System.Collections.Generic;

namespace System.Net.Torrent
{
	public class PeerCommand
	{
		public Int32 CommandLength { get; private set; }
		public byte CommandID { get; private set; }
		public bool IsExtCommand { get { return CommandID == 20; } }
		public byte ExtCommandID { get; private set; }
		public byte[] CommandPayload { get; private set; }

		public PeerCommand()
		{
			
		}

		public PeerCommand(byte[] commandBytes)
		{
			CommandLength = Unpack.Int32(commandBytes, 0, Unpack.Endianness.Big);
			Init(CommandLength, MicroLinq.Skip(commandBytes, 4));
		}

		public PeerCommand(Int32 commandLength, byte[] commandBytes)
		{
			Init(commandLength, commandBytes);
		}

		private void Init(Int32 commandLength, IEnumerable<byte> commandBytes)
		{
			byte[] buffer = commandBytes as byte[];

			if(buffer == null) return;

			CommandLength = commandLength;
			CommandID = buffer[0];
			if (IsExtCommand)
			{
				ExtCommandID = buffer[2];
				CommandPayload = MicroLinq.Skip(commandBytes, 2) as byte[];
			}
			else
			{
				CommandPayload = MicroLinq.Skip(commandBytes, 1) as byte[];
			}
		}
	}
}