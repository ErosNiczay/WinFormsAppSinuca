using System.Collections.Generic;

namespace WinFormsAppSinuca
{
	public class Tabela : ITabela
	{
		//declarations
		public string Nome { get; set; }
		public string Premiacao { get; set; }
		public string Regra { get; set; }
		public List<ICompetidor> Competidores { get; set; }
		public int Times { get; set; }
		public int Vitoria { get; set; }

		public ICompetidor Vencedor
		{
			get
			{
				ICompetidor vencedor = null;
				foreach(var competidor in this.Competidores)
				{
					if (competidor.Pontos >= this.Vitoria)
					{
						vencedor = competidor;
						break;
					}
				}
				return vencedor;
			}
		}

		//constructor
		public Tabela()
		{
			this.Nome = "";
			this.Premiacao = "";
			this.Regra = "";
			this.Competidores = new List<ICompetidor>();
			this.Times = 10;
			this.Vitoria = 0;
		}
	}
}
