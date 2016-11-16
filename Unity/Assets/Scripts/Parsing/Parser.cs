using System;

namespace Parsing
{
	public interface Parser
	{
		bool CanParse(string msg);
		Command Parse(string msg);
	}
}