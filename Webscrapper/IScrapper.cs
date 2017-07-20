using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Webscrapper
{
	public interface IScrapper
	{
		string  Name { get;}
		bool Check();
	}
}
