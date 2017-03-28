using System;
/*
    @author Jamie Redding (jgr2)
*/

namespace Parsing
{
	public interface Parser
	{
		bool CanParse(string msg);
		Command Parse(string msg);
	}
}