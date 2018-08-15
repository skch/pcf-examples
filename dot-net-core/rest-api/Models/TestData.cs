using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestApiShowcase.Models
{
  public class TestData
  {
  	public string environment { get; set; }
		public string name { get; set; }
		public string version { get; set; }
		public string CFID { get; set; }
  	public string status { get; set; }
  	public string message { get; set; }
  }
}

