﻿/********************************************************************************/
/* Projeto: Biblioteca ZeusNFe                                                  */
/* Biblioteca C# para emissão de Nota Fiscal Eletrônica - NFe e Nota Fiscal de  */
/* Consumidor Eletrônica - NFC-e (http://www.nfe.fazenda.gov.br)                */
/*                                                                              */
/* Direitos Autorais Reservados (c) 2014 Adenilton Batista da Silva             */
/*                                       Zeusdev Tecnologia LTDA ME             */
/*                                                                              */
/*  Você pode obter a última versão desse arquivo no GitHub                     */
/* localizado em https://github.com/adeniltonbs/Zeus.Net.NFe.NFCe               */
/*                                                                              */
/*                                                                              */
/*  Esta biblioteca é software livre; você pode redistribuí-la e/ou modificá-la */
/* sob os termos da Licença Pública Geral Menor do GNU conforme publicada pela  */
/* Free Software Foundation; tanto a versão 2.1 da Licença, ou (a seu critério) */
/* qualquer versão posterior.                                                   */
/*                                                                              */
/*  Esta biblioteca é distribuída na expectativa de que seja útil, porém, SEM   */
/* NENHUMA GARANTIA; nem mesmo a garantia implícita de COMERCIABILIDADE OU      */
/* ADEQUAÇÃO A UMA FINALIDADE ESPECÍFICA. Consulte a Licença Pública Geral Menor*/
/* do GNU para mais detalhes. (Arquivo LICENÇA.TXT ou LICENSE.TXT)              */
/*                                                                              */
/*  Você deve ter recebido uma cópia da Licença Pública Geral Menor do GNU junto*/
/* com esta biblioteca; se não, escreva para a Free Software Foundation, Inc.,  */
/* no endereço 59 Temple Street, Suite 330, Boston, MA 02111-1307 USA.          */
/* Você também pode obter uma copia da licença em:                              */
/* http://www.opensource.org/licenses/lgpl-license.php                          */
/*                                                                              */
/* Zeusdev Tecnologia LTDA ME - adenilton@zeusautomacao.com.br                  */
/* http://www.zeusautomacao.com.br/                                             */
/* Rua Comendador Francisco josé da Cunha, 111 - Itabaiana - SE - 49500-000     */
/********************************************************************************/

using System;
using DFe.Configuracao;
using DFe.DocumentosEletronicos.Flags;
using DFe.DocumentosEletronicos.ManipuladorDeXml;
using DFe.DocumentosEletronicos.ManipulaPasta;
using DFe.DocumentosEletronicos.MDFe.Validacao;
using DFe.DocumentosEletronicos.NFe.Classes.Servicos.Autorizacao;
using DFe.DocumentosEletronicos.NFe.Configuracao;

namespace DFe.DocumentosEletronicos.NFe.Classes.Extensoes
{
    public static class ExtenviNFe3
    {
        /// <summary>
        ///     Converte o objeto enviNFe3 para uma string no formato XML
        /// </summary>
        /// <param name="pedEnvio"></param>
        /// <returns>Retorna uma string no formato XML com os dados do objeto enviNFe3</returns>
        public static string ObterXmlString(this enviNFe3 pedEnvio)
        {
            return FuncoesXml.ClasseParaXmlString(pedEnvio);
        }

        public static void ValidarSchema(this enviNFe3 pedStatus, NFeBaseConfig config)
        {
            var xml = pedStatus.ObterXmlString();

            switch (config.VersaoNfeStatusServico)
            {
                case VersaoServico.Versao310:
                    Validador.Valida(xml, "enviNFe_v3.10.xsd", config);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(pedStatus));
            }
        }

        public static void SalvarXmlEmDisco(this enviNFe3 enviNFe3, DFeConfig dfeConfig, int idLote)
        {
            if (dfeConfig.NaoSalvarXml()) return;

            var caminhoXml = new ResolvePasta(dfeConfig, DateTime.Now).PastaCanceladosEnvio();

            var arquivoSalvar = caminhoXml + $"\\{idLote}-env-lot.xml.xml";

            FuncoesXml.ClasseParaArquivoXml(enviNFe3, arquivoSalvar);
        }
    }
}