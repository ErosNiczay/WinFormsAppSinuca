using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;
using System.Drawing;

namespace WinFormsAppSinuca
{
	public partial class FormSinuca : Form
	{
		//declarations
		private List<ITime> Times;
		private List<ITabela> Tabelas;

		//constructor
		public FormSinuca()
		{
			InitializeComponent();
			this.Load += FormSinuca_Load;
			this.tabSinuca.SelectedIndexChanged += TabSinuca_SelectedIndexChanged;

			this.btnTimeSalvar.Click += BtnTimeSalvar_Click;
			this.btnTimeExcluir.Click += BtnTimeExcluir_Click;

			this.btnTabelaSalvar.Click += BtnTabelaSalvar_Click;
			this.btnTabExcluir.Click += BtnTabExcluir_Click;

			this.lvwCampTabelas.ItemSelectionChanged += LvwCampTabelas_ItemSelectionChanged;
			this.btnCampTimeAdicionar.Click += BtnCampTimeAdicionar_Click;
		}

		//events
		private void FormSinuca_Load(object sender, System.EventArgs e)
		{
			this.Times = new List<ITime>();
			this.Tabelas = new List<ITabela>();
		}
		private void TabSinuca_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			switch (tabSinuca.SelectedIndex)
			{
				case 0:
					CarregarCampeonatoTabelas();
					LimparCampeonatoTabelaDetalhes();
					break;
				case 1:
					LimparTabelaDetalhes();
					CarregarTimesLista();
					break;
				case 2:
					LimparTimeDetalhes();
					CarregarTimesLista();
					break;
			}
		}

		//methods
		/// Times
		private void LimparTimeDetalhes()
		{
			txtTimeNome.Text = "";
			txtTimeJogador1.Text = "";
			txtTimeJogador2.Text = "";
		}
		private void CarregarTimesLista()
		{
			lvwTimeLista.Items.Clear();
			foreach (var time in this.Times)
			{
				var linha = lvwTimeLista.Items.Add(time.Nome);
				linha.Tag = time;
				linha.SubItems.Add(time.Jogador1);
				linha.SubItems.Add(time.Jogador2);
			}
		}
		private void BtnTimeSalvar_Click(object sender, System.EventArgs e)
		{
			if (string.IsNullOrWhiteSpace(txtTimeNome.Text)) return;
			if (string.IsNullOrWhiteSpace(txtTimeJogador1.Text)) return;
			if (string.IsNullOrWhiteSpace(txtTimeJogador2.Text)) return;

			this.Times.Add(new Time()
			{
				Nome = txtTimeNome.Text.Trim(),
				Jogador1 = txtTimeJogador1.Text.Trim(),
				Jogador2 = txtTimeJogador2.Text.Trim()
			});

			MessageBox.Show("Time Salvo.");
			LimparTimeDetalhes();
			CarregarTimesLista();
		}
		private void BtnTimeExcluir_Click(object sender, System.EventArgs e)
		{
			if (lvwTimeLista.FocusedItem == null) return;

			if (MessageBox.Show("Deseja excluir este time?", "Excluindo", MessageBoxButtons.YesNo) == DialogResult.Yes)
			{
				this.Times.Remove((Time)lvwTimeLista.FocusedItem.Tag);
				MessageBox.Show("Time excluido.");
				CarregarTimesLista();
			}
		}
		/// Times

		/// Tabelas
		private void LimparTabelaDetalhes()
		{
			txtTabNome.Text = "";
			txtTabPremio.Text = "";
			txtTabRegra.Text = "";
			numTabVitoria.Value = numTabVitoria.Minimum;
			numTabTimes.Value = numTabTimes.Minimum;
			CarregarTabelasTimes();
		}
		private void CarregarTabelasTimes()
		{
			lstTabCompetidores.Items.Clear();
			foreach (var time in this.Times)
			{
				lstTabCompetidores.Items.Add(time);
			}
		}
		private void CarregarTabelaLista()
		{
			lvwTabLista.Items.Clear();
			foreach (var tabela in this.Tabelas)
			{
				var linha = lvwTabLista.Items.Add(tabela.Nome);
				linha.Tag = tabela;
				linha.SubItems.Add(tabela.Premiacao);
				linha.SubItems.Add(tabela.Regra);
				linha.SubItems.Add(tabela.Vitoria.ToString());
				linha.SubItems.Add(tabela.Times.ToString());
				var sub = linha.SubItems.Add("");
				foreach (var competidor in tabela.Competidores)
				{
					sub.Text += competidor.ToString() + " / ";
				}
				sub.Text = sub.Text[0..^3];
			}
		}
		private void BtnTabelaSalvar_Click(object sender, System.EventArgs e)
		{
			if (string.IsNullOrWhiteSpace(txtTabNome.Text)) return;
			if (string.IsNullOrWhiteSpace(txtTabPremio.Text)) return;
			if (string.IsNullOrWhiteSpace(txtTabRegra.Text)) return;
			if (lstTabCompetidores.CheckedItems.Count < 2) return;
			if (lstTabCompetidores.CheckedItems.Count > numTabTimes.Value) return;

			List<ICompetidor> competidores = new List<ICompetidor>();
			foreach (var selecionado in lstTabCompetidores.CheckedItems)
			{
				competidores.Add(new Competidor() { Time = (Time)selecionado, Pontos = 0 });
			}
			this.Tabelas.Add(new Tabela()
			{
				Nome = txtTabNome.Text.Trim(),
				Premiacao = txtTabPremio.Text.Trim(),
				Regra = txtTabRegra.Text.Trim(),
				Vitoria = (int)numTabVitoria.Value,
				Times = (int)numTabTimes.Value,
				Competidores = competidores
			});

			MessageBox.Show("Tabela Salva.");
			LimparTabelaDetalhes();
			CarregarTabelaLista();
		}
		private void BtnTabExcluir_Click(object sender, System.EventArgs e)
		{
			if (lvwTabLista.FocusedItem == null) return;

			if (MessageBox.Show("Deseja excluir esta tabela?", "Excluindo", MessageBoxButtons.YesNo) == DialogResult.Yes)
			{
				this.Tabelas.Remove((Tabela)lvwTabLista.FocusedItem.Tag);
				MessageBox.Show("Tabela excluída.");
				CarregarTabelaLista();
			}
		}
		/// Tabelas

		/// Campeonato
		public void CarregarCampeonatoTabelas(ITabela atual = null)
		{
			lvwCampTabelas.Items.Clear();
			foreach (var tabela in this.Tabelas)
			{
				var linha = lvwCampTabelas.Items.Add(tabela.Nome);
				linha.Tag = tabela;
				linha.SubItems.Add(tabela.Vencedor?.Time.Nome ?? "");
				linha.SubItems.Add(tabela.Vencedor?.Pontos.ToString() ?? "0");
				if (tabela == atual)
				{
					linha.Selected = true;
					linha.Focused = true;
				}
				if(tabela.Vencedor!=null)
				{
					linha.ForeColor = Color.Green;
				}
			}
		}
		public void LimparCampeonatoTabelaDetalhes()
		{
			lblCampTabPremio.Text = "Premiação";
			lblCampTabRegra.Text = "Regras";
			lblCampTabVitoria.Text = "0";
			lblCampTabTimes.Text = "0";
			lvwCampTimes.Items.Clear();
			numCampTimePontos.Value = numCampTimePontos.Minimum;
			btnCampTimeAdicionar.Enabled = false;
		}
		public void CarregarCampeonatoTimes(List<ICompetidor> competidores)
		{
			lvwCampTimes.Items.Clear();

			competidores = competidores.OrderByDescending(x => x.Pontos).ToList();

			foreach (var competidor in competidores)
			{
				var linha = lvwCampTimes.Items.Add(competidor.Time.Nome);
				linha.Tag = competidor;
				linha.SubItems.Add(competidor.Pontos.ToString());
				linha.SubItems.Add(competidor.Time.Jogador1);
				linha.SubItems.Add(competidor.Time.Jogador2);
			}
		}
		private void LvwCampTabelas_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
		{
			if (!e.IsSelected) return;

			var tabela = (Tabela)e.Item.Tag;
			lblCampTabPremio.Text = "Premiação: " + tabela.Premiacao;
			lblCampTabRegra.Text = "Regras: " + tabela.Regra;
			lblCampTabVitoria.Text = "Vitória: " + tabela.Vitoria.ToString() + " pontos";
			lblCampTabTimes.Text = "Times: " + tabela.Times.ToString();
			CarregarCampeonatoTimes(tabela.Competidores);
			numCampTimePontos.Value = numCampTimePontos.Minimum;
			numCampTimePontos.Maximum = tabela.Vitoria;
			btnCampTimeAdicionar.Enabled = tabela.Vencedor == null;
		}
		private void BtnCampTimeAdicionar_Click(object sender, System.EventArgs e)
		{
			if (lvwCampTabelas.FocusedItem == null) return;
			if (lvwCampTimes.FocusedItem == null) return;

			var tabela = this.Tabelas.First(x => x.Equals((Tabela)lvwCampTabelas.FocusedItem.Tag));
			var competidor = tabela.Competidores.First(x => x.Equals((Competidor)lvwCampTimes.FocusedItem.Tag));
			competidor.Pontos += (int)numCampTimePontos.Value;

			CarregarCampeonatoTabelas(tabela);
		}
		/// Campeonato
	}
}
