using System.Collections.Generic;

namespace WinFormsAppSinuca
{
	public interface ITabela
	{
		public string Nome { get; set; }
		public string Premiacao { get; set; }
		public int Vitoria { get; set; }
		public string Regra { get; set; }
		public List<ICompetidor> Competidores { get; set; }
		public int Times { get; set; }
		public ICompetidor Vencedor { get; }
	}
}
