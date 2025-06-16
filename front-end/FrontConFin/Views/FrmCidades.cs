using FrontConFin.Models;
using FrontConFin.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FrontConFin.Views
{
    public partial class FrmCidades : Form
    {
        private PaginacaoResponse<Cidade> paginacao = null;
        private bool novo = false;
        public FrmCidades()
        {
            InitializeComponent();
            dataGridView1.AutoGenerateColumns = false;
            carregaComboEstado();
            atualizaDados();
        }

        private void verificaBotoes()
        {
            buttonSalvar.Enabled = false;
            buttonCancelar.Enabled = false;
            buttonNovo.Enabled = permissaoUsuario();
            buttonAlterar.Enabled = false;
            buttonExcluir.Enabled = false;
            buttonPrimeira.Enabled = false;
            buttonAnterior.Enabled = false;
            buttonProxima.Enabled = false;
            buttonUltima.Enabled = false;
            buttonPesquisar.Enabled = true;

            if (paginacao.TotalLinhas > 0)
            {
                buttonPrimeira.Enabled = true;
                buttonAnterior.Enabled = true;
                buttonProxima.Enabled = true;
                buttonUltima.Enabled = true;
                buttonAlterar.Enabled = permissaoUsuario();
                buttonExcluir.Enabled = permissaoUsuario();
            }
        }

        private bool permissaoUsuario()
        {
            if (UsuarioSession.Funcao == "Gerente" || UsuarioSession.Funcao == "Empregado")
            {
                return true;
            }
            return false;
        }

        private async void atualizaDados()
        {
            int skip = int.Parse(textBoxSkip.Text);
            int take = int.Parse(textBoxTake.Text);
            paginacao = await CidadeServices.Paginacao(textBoxPesquisa.Text, skip, take, checkBoxOrdem.Checked);
            dataGridView1.DataSource = paginacao.Dados;
            atualizaCamposDetalhes();
            verificaBotoes();
        }

        private async void carregaComboEstado()
        {
            comboBoxEstado.DataSource = await EstadoServices.GetEstados();
        }

        private void atualizaCamposDetalhes()
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                Cidade cidade = (Cidade)dataGridView1.SelectedRows[0].DataBoundItem;
                textBoxId.Text = cidade.Id.ToString();
                textBoxNome.Text = cidade.Nome;

                int indice = 0;
                for (int i = 0; i < comboBoxEstado.Items.Count; i++)
                {
                    Estado estado = (Estado)comboBoxEstado.Items[i];
                    if (estado.Sigla == cidade.EstadoSigla)
                    {
                        indice = i;
                        break;
                    }
                }
                comboBoxEstado.SelectedIndex = indice;
            }
        }

        private void habilitaCampos()
        {
            if (novo)
            {
                textBoxId.ReadOnly = true;
                textBoxNome.ReadOnly = false;
                comboBoxEstado.Enabled = true;
                textBoxId.Clear();
                textBoxNome.Clear();
            }
            else
            {
                textBoxId.ReadOnly = true;
                textBoxNome.ReadOnly = false;
                comboBoxEstado.Enabled = true;
            }
        }

        private void desabilitaCampos()
        {
            textBoxId.ReadOnly = true;
            textBoxNome.ReadOnly = true;
            comboBoxEstado.Enabled = false;
        }

        private void habilitaBotoes()
        {
            buttonNovo.Enabled = false;
            buttonAlterar.Enabled = false;
            buttonExcluir.Enabled = false;
            buttonSalvar.Enabled = true;
            buttonCancelar.Enabled = true;
            buttonPrimeira.Enabled = false;
            buttonAnterior.Enabled = false;
            buttonProxima.Enabled = false;
            buttonUltima.Enabled = false;
            buttonPesquisar.Enabled = false;
        }

        private void buttonPrimeira_Click(object sender, EventArgs e)
        {
            if (paginacao.Skip > 1)
            {
                textBoxSkip.Text = "1";
                atualizaDados();
            }
        }

        private void buttonAnterior_Click(object sender, EventArgs e)
        {
            if (paginacao.Skip > 1)
            {
                paginacao.Skip--;
                textBoxSkip.Text = paginacao.Skip.ToString();
                atualizaDados();
            }
        }

        private async void buttonSalvar_Click(object sender, EventArgs e)
        {
            Estado estado = (Estado)comboBoxEstado.SelectedItem;
            Cidade cidade = new Cidade()
            {
                Nome = textBoxNome.Text,
                EstadoSigla = estado.Sigla
            };

            if (!novo)
            {
                cidade.Id = Guid.Parse(textBoxId.Text);
            }

            var resultado = novo ? await CidadeServices.PostCidade(cidade) : await CidadeServices.PutCidade(cidade);

            if (resultado)
            {
                tabControl1.SelectedTab = tabPage1;
                atualizaDados();
                desabilitaCampos();
            }
        }

        private void buttonSair_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void buttonPesquisar_Click(object sender, EventArgs e)
        {
            atualizaDados();
        }

        private void FrmCidades_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                buttonPesquisar_Click(sender, e);
            }
        }

        private void buttonProxima_Click(object sender, EventArgs e)
        {
            decimal paginas = paginacao.TotalLinhas / paginacao.Take;
            int quantidadePaginas = (int)Math.Ceiling(paginas);
            if (paginacao.Skip < quantidadePaginas)
            {
                paginacao.Skip++;
                textBoxSkip.Text = paginacao.Skip.ToString();
                atualizaDados();
            }
        }

        private void buttonUltima_Click(object sender, EventArgs e)
        {
            decimal paginas = paginacao.TotalLinhas / paginacao.Take;
            int quantidadePaginas = (int)Math.Ceiling(paginas);
            if (paginacao.Skip < quantidadePaginas)
            {
                textBoxSkip.Text = quantidadePaginas.ToString();
                atualizaDados();
            }
        }

        private void buttonPrimeira_Click_1(object sender, EventArgs e)
        {
            if (paginacao.Skip > 1)
            {
                textBoxSkip.Text = "1";
                atualizaDados();
            }
        }

        private void buttonAnterior_Click_1(object sender, EventArgs e)
        {
            if (paginacao.Skip > 1)
            {
                paginacao.Skip--;
                textBoxSkip.Text = paginacao.Skip.ToString();
                atualizaDados();
            }
        }

        private void buttonCancelar_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedTab = tabPage1;
            atualizaCamposDetalhes();
            verificaBotoes();
            desabilitaCampos();
        }

        private void buttonNovo_Click(object sender, EventArgs e)
        {
            novo = true;
            habilitaBotoes();
            habilitaCampos();
            tabControl1.SelectedTab = tabPage2;
        }

        private void buttonAlterar_Click(object sender, EventArgs e)
        {
            novo = false;
            habilitaBotoes();
            habilitaCampos();
            tabControl1.SelectedTab = tabPage2;
        }

        private async void buttonExcluir_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                Cidade cidade = (Cidade)dataGridView1.SelectedRows[0].DataBoundItem;
                DialogResult result = MessageBox.Show(null, $"Deseja excluir a cidade {cidade.Nome}?", "Controle Financeiro", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (result == DialogResult.OK)
                {
                    var resultado = await CidadeServices.DeleteCidade(cidade.Id);
                    if (resultado)
                    {
                        tabControl1.SelectedTab = tabPage1;
                        atualizaDados();
                    }
                }
            }
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            atualizaCamposDetalhes();
        }
    }
}
