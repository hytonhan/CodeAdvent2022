using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeAdvent2022
{
    internal class Day03 : IDay
    {
        public int Order => 3;

        //const string _inputFilename = "day03_input.txt";

        const int letterCount = (int)('z' - 'a');
        public void Run()
        {
            char foo = 'a';
            char bar = 'A';

            Console.WriteLine($"a: {(int)foo}. A: {(int)bar}. a+25: {(char)(foo + 25)}. A + 25: {(char)(bar + 25)}");

            int sum = 0;
            string[] lines = _input.Split(Environment.NewLine);
            int i = 0;
            int elfGroups = 0;
            string elfGroup = "";
            foreach (string line in lines)
            {
                i++;
                if (i == 1)
                {
                    elfGroup = line;
                }
                if (i == 2)
                {
                    elfGroup = new string(elfGroup.Intersect(line).ToArray());
                }
                if (i == 3)
                {
                    i = 0;
                    elfGroups += CharToPriorityConverter(elfGroup.Intersect(line).Single());
                }
                var ruckSackLength = line.Length;

                var firstCompartment = line.Substring(0, ruckSackLength / 2);
                var secondCompartment = line.Substring(ruckSackLength / 2);

                var common = firstCompartment.Intersect(secondCompartment).Single();
                sum += CharToPriorityConverter(common);
            }

            Console.WriteLine($"Sum of priorities: {sum}");
            Console.WriteLine($"Sum of elfgroups: {elfGroups}");
        }

        int CharToPriorityConverter(char character)
        {
            if ((int)character - 'a' >= 0 && (int)character - 'a' <= letterCount)
            {
                return (int)character - 'a' + 1;
            }
            if ((int)character - 'A' >= 0 && (int)character - 'A' <= letterCount)
            {
                return (int)character - 'A' + 2 + letterCount;
            }
            return -1;
        }

        private const string _input = @"QLFdFCdlLcVqdvFLnFLSSShZwptfHHhfZZZpSwfmHp
rTJRjjbJTgzDJjdsRsfwtfNwtfmZpZNhmmzt
bMdJjsjglnVMFCCc
BZvZMBBBMTtZTgcCPdgtgQCrrV
VspNDDpsGDGnRmRpRplQdrrPcPCjgDCcPQCQQj
RVnsmspnpwFRlHGRwHHlnRSThSSvBTbFTZMqTMZMTZFh
ttffrVJWtWpgtQnZGVnNSLTHSZ
jRsjjmhdRcjcRsFcdwGLMZSnHMTSMNZN
RjczlvjhPCcPjcvRpbglWplJBblqrGgq
NwCWwdNQNDCwGpWNQwmJtZgPZrdJgLZcPhLddr
blqpnFTqrLbcLPtV
MnjqSSpqlFRvSqNDGHvWHQDwfWmm
jfLljlQhDLmlrMJQtJTJrQqQ
NpBbjjsdMCgCCMrb
dwspwGnSHHGsGzDDlFDjVWjfZWnP
wQhTZwvpZFZdqWLnnwSrWC
mfDmMFlDcPLdgDSCLCqg
PmzclsMclMlFsHHsJZFHpT
LfLJWNdJnBLfhndfWdZqcgDZgSqgCCSSSLDF
bQVQmrrjPqQDZSZBCQ
RRtGjVmRsPbPrrnNNpNHHnBnpHns
PfbGNwGBwNcPTbGNQFBVjsjztVztVjHV
hrdCJhmlJhZrLDRmghrmDrzqFsbgtbHqnznzznQtbjtz
WdZdDJCDmrJmLZrLDLDZlClcSccwSPbNPfSNWfGNNWMGNc
QwrnTSgqgFShSdfHPdbS
BGdjmMmZMvfhvCZZPf
BzGmzVGGGzmzGpVBtdnQqqdTQQDDqrpR
PPRPwLQlLtbPmbwgJhrSssNlhhrgsZ
fFTdFvTMNfzVnFqdnjgSSjjsSjhghpJs
dvczcFzNTWVWMFLcLbQQwmbHLCLL
HhLLDfMmTjsjmLmhsmmfZMjGtpGJtdcvnCWtZJcWGddttW
gwrwwgzwgDpJddrJDr
SBwBBbgVsVmRRhDM
SZdmfdZRTQZTQgHVVGGRqZdVCjCcNCNcJRWcCBbJjCPCsNsc
FnhzMMhDDFlzlnvpwMLrMDCsbcjNJbcJbBcBfPhCNbWj
wzwnpDDnLvFnLlttLrzGgTVGQqZtTqSqSdfZTg
FJJWWWrCGWdmzFlTVqqlMhmvVlNh
btpgtfjZjjhgggrNMThl
DpwpfRZDZwwfwjsnjfsZnnnwGcCRCHcCCCLGHLWcrcFCWCHW
ljHHHBtjQthchhZpqqNt
FTmJnPFwzlJPmzTgTgbFwJbMCpbMchhhqbhWCDMDhZcppM
JwlFGwVGnFFGBGjdSsfdsG
QsvpGhjGgswvjjwjjjvPpThJfLZCCLCSSLbFLStCFCSgtH
mDrzdzMqqnMrDMmZnqnzNVRCStlCHFSCtJqlCLFCFLfJfJ
mBDzNRWBDDMBpGsZcGZWGjPp
SlLQhQsvvttFlWsWcfHHMTJfwSHwTfTf
VZmmrRDRfpTHJcRf
jzBnDDgjPchWlsQsBW
LTLVdTSLNTLnTSnrWvdwswsfmJwmwGsffH
FbgCbRRppCpPbgMcZvCcGftGGltwHwGtplQQsfJw
CRBMCvZZRgMgBbDCPcDrjLzWLVrSSzShSSNrBS
hVJJjhjRVRZjQvDfBstsBVNBdwstHsld
pCTCcMqCThTFLFFPWcWSPHtwwdmcBHHmNtHdmwmwBl
rMTCCWPLLPCMFhDnDrjzRrfDJD
pqMpCvMchvFNWSBdVNqQ
zDRJJDGJJtNtmGRRWVdFWWVdSfjb
DDJLmnJmzwGmGLTPhTCNpgcrpv
cpPpbPWVprWcbJrrwpCwwdWrvNNFRqzNnChgqFzFnZvqFlzq
fTtHLfSHSsNDGLSmsLvnhFqzhzlzDhhvRlFz
mSMLHTQTmHMSfBSMTPdBJJNNVddrVrbjbJ
zpCpBTnFgFbncznbblzdhRswdlJsLllJdw
QqqmtWVPWvHDVmqDhjsljwRhlZldhRMQ
tWSHDmVfmrtPHVgGRRbgTRpSgpTc
ssTbzFRtPRwTFZtvbPRMhndBqMMvMBHJnnHMMd
WQVWzlGWVBqqdMQJMq
pVSpSSgLfjDzWrLGWWjDzzfLtbRFFNtPZRssspsNRZcRsNZb
jnPzzGlnnznWnzhvGnnpVFrZmVFcgjrrmZRFtj
fsbgTdwdqBbfwCptVtdZRcrRCp
gsMbgfHsBSwsGhhJWMLnWPPJ
bpmbJpNbbbNGGmRJzJTsfdsvsNdglfhssCvC
hWLwQjZjLhjHFFBLZldvvflrvtfjCrfjlT
QWVQZZFDcDJJhJJc
RmRghgRlNgfGGRmdGqhsgsZFZZpBvHpZppHcgH
tbLCDLnLtSbbbjtPtMLtDPTvHHBpcHcsHvTcHsmZcF
rSLrMJzDznzGmhNlVwdrRr
vWjljMWcnSSpjmzbJVzJrTCmtGJV
NZDDQLRqPJrPzrprTC
gqDqqwpdHWhlgnjH
ccptcpstDvbNvHbLNRZZ
dFjhdnjQFJlFCQSjgngJPJgWWrRNWNRtNCzrVbRzNVzZZL
PhThFSPPSpsmTcqMwt
cLcLlMhGMGcpGzslHFHFvnHlBDvWbT
VHdQwqPJdPwjJPdPQRrmjjjQnTFrbvNWFFffDvbvvNDvbBNN
RwRJCHmmdQJZZLzGphcCtz
hVvFVjvjVWmFRQVZqTpqtwQpwpqZfp
gvDlSBDJSlPLcLdDwzwtptqTTTzwcCCt
JgrJGbLgvnWsmvVr
rwmqqRqrnHQGmnjCCqCzdBzCBJBz
hFLgbWWPWmvtLhPtgpcdjJdcBJdpJjDsgp
lvfhSSPWtTNTTZZmfr
bHDDssRHsjNMbJjJLQJsbTtGvSCzCGQCTzGvSqSBzT
mmVrwhmmpfPStnTSBnhStG
pcwrptZcgFcmpgHRDjjZlDJsjbbD
JJRrmFqJMdFFJMjjJcqGgzSCSHSCscPCHPHGZc
VWpWptnvSmpPGCHC
vQnDLBmbntvLBbnlldTQFlJlFFrNRd
LPDftnHFQfwmBcBGmc
CVqRsdqvdrlsCVsNvqwwSpTNSSDSDDBBTTSN
lqlDRddjbblRbRqrlRRjsbvghHHnPQWjHWQWZHPHWZhFnP
bwQsDcgsJqcsDpcQRQnpqtVSVvgSMMMfMvfVBVfdvM
CGZFrHHPrTZNGGZZHmCZHlVfjfzjSfzBtBBNSBVjvntf
ChrCCLGrTlhJnhDncRbp
nmFnhfTQjSzfjddZWsRRRFRFGl
HDgCwgtQbZlHsrqHHr
cJPCgCCPbpbgDMPvMQnjmnhTfmzLpQQmjz
QFHSQdNMCSgcSgFtttPNFJpCpnTjZlbblpppZplZjz
LqLsWMRRfrrWMmMGpbTnbppbZnTjpnmm
fWBrMqWsGswGfGRMMwrLgtPPdNFBQPHNHNPPcFPP
dngbSppJSSppbVMZQQMjqfQQgwcl
TWmSWtvCRCWjwfjqQqMstq
FhFvRzSTNmhHnVPhGhBJdBpB
gcHPgzGmPPwTsSTsbwbdWD
QjBLLfVhhBqqBFQLrLjVFlNpNDtsSWTDdNptdbqbdS
jCMFLVjFBFjJJLFFMVBFrLnvPzHRmHPGnGWWcCvzHRZm
PDPqWWjhPpPbCsjwjTVbLT
SrtCttGRMddSVwHFSs
JtfvttmrGMRRJzJCqhqqqWQZhCNgJZ
ChrCVFQCVQlwQNwpQcmmcjmWBmddghjjdW
sbDTZStTqqfSBggPmWjWWNsL
THqqSHDTtZTDTHZZbHTzRzFvlFCVprFprQVnCzNppQrz
PdfWCwMWjPSrdgCMnnlGsGQvvpJZvFGnps
DmBhVBLbbVqVBzTRLBRzzTLNNpRQNNZQZppZvlQpZvllvF
zVDtVHBbbTbzDbrjgWjMPtMWPMlj
JLsTTNDsgTMNvDQpLpGpLGNJShrfzCFnSnSrnfzCfTFhWrfw
ZcqrRddHZZVRfzWnVWCzWFnn
tZHZtrHHPdRtdHlcccQggsplpJDNvMGNGMss
cMCLfStfMTCjPMPcGzjftMbgsRNmRgmmGsmnJbNJbghJ
QHVVWrFFWZNShHSgbSJm
qZwwrrpqpZpZFvqrQdFlQVSwLBMBfTTLTjLBCcdTMzMftPPB
SwsdBTvgvJLPNptpCpCmBDtn
wffrzwGFWFNZWpjWZnNm
zrfflbRwJPhbPbsS
HjHHRtwjnjRblQRttHwQGvGWNNBWvqGzfTvfNN
FmScCcrsdVZrpBrVcCVFzffvzzmWGLWgqWqgGWzW
SFVSDDBdsdDSJhnjJltJbPtHRM
FjGFVqWrzQFlQrZzGQzFLTvfwwTgMnvcnbRMLRdnfb
CCttSNsSnRfgncSg
CNspmDBPtPmJJNBJPNpDhQZVzQlhqrGZflfVjQFrQj
dNNdHWcmdmPPptmmWHpPTFFjJPGrQsVsPQGGGJVDrVVGrS
MhZlZhlgflgfnfDtjbjJGbtnVtGS
LtZqlzhzqMZWHHLwdHmFWp
llNRlfwWRwwLlwFNNgRrVCBjdjCVdjpWjtVWCD
HTQqzPqzQPmhhmSPznSsssJtdnMZddtMCjprtMjCnBVnjZ
PzHQmqsGSJPSmQqPbfwNcgNbNgNfBGwR
lPdzlZPzQzMZQGQrTZvvpjHTTpfsTTZb
zRShhtWRnqnqSNRnDTTHvfNJspNsLpTsjL
hBVncVtDSnhDnDBBtGrlzwmmMlGmVrMdrP
HPTZVHVPlHDPlfgnjJFdJdjPjSPqCS
hLRRBhwGhqbtmsRSSSjjdMJjnJGSMj
QrQtqrRrcQDgVglc
ZTwbbZdchZZjmVWHTrHWBVJtBB
glslCDqLLDfGRqlsgLssfrCHBHFHmrHBBppFmCJWWp
fRzvvvgGgNSNvmQbSQ
qPGGPwCTqTzHCvPGqWdLFLssLpstLLspvd
njJchhcbjbDrbcLNlLrpWWrLLHgp
DQhMMMJQMQJnVbbnRHSMPwZmGZPZRTRCwTZmZGwz
zzGNfPbcgdPqLrqvWWVzMq
DGmJtnJTJRhhJMhCQqCLCLrrLM
ZnHDtSZlTBHnBdccGSfGcwjjdb
FpZDpQZDvMwZpCCMdCBPpJGPPLgJGGLffJJL
jlbswNrlPPJJfGlf
bnNwqbHnNwRSrqhbdCcmHddQzddFDdvZ
gbQQQngWPVVtvvPQNVNvWWSHGwDsCCmDtHSlmrssDmHs
fqhMLFFMMZqZMRZqMjRMqLJSCdFlrrldsDrCsDSSHHGCSC
MJRZLZLGMcTqczjNPzNnzPvWBVgNnP
gqdbBffTvlRHbwLl
nMMQJQpGdsFpQsJzNMRLLDlmLLmjFFmLjDRF
pzGMnVcMBfTdtBBV
WSbfmrrrrWdbWmdfDSSStmHjtMtvCLVnqBHCVGtVGnMM
lRcgFRZhJgnMLjvGgv
lcvwTcFTplvwphzcTTJTbsdsPSPDdbmzmDSWPsSm
bbdTjTQTQMsZNqqhJrZslg
jFGVjwfCPVGfwjCVqWhWZFgqWrglllNN
PjfSPzRBjCCfSBCGBLznTndHcdMLbMmmdT
wSVMJSVccdGwGnsgbVTTbRsCRNgN
rHjhHLmrhPJrqjNTRDgBbbRRRs
zqmPPqqpPLzltrMdJcZpfdpGWWJJ
ZhrBBJGrgJhGHttGGVPPcPPF
cnzLqNssfRnpfWqsLfcfWQNMbMVPDtnDtbHFtMbPtVPFFM
jfqzCCLsWQLcjgldjmljmgTd
wghGSSGZPVwgqtwtwCCtFFMM
BvbspnBznvvWHWHHHbCQptQFQlFcqMClqLLq
JWzzsJHWzfWjJrvMBWHBBGDmVDrVhZmmgSPSmZVVrh
ccRMJRsjjgJgcPCSCCVCwsSWVNzp
WQQqnmrBWtqWqdSbVwwBSpbbCSBB
QvDqmqqmgWPWjPvW
msqpjDWspRWwvFvDWWhnbbJfPzFQblJJPlnz
gGGrMTgLVBsBBLdsVTrSCBffHQfdHhnbPPPPffndlbzh
ZCVsCGSScsLZpwNpqmZRqW
PPsGmJPVPQPZmsQCVPJPnPCMDcTcdqDDTqvFhvnTjRDTDchq
BdrtzNBLHStHrdrlwfNThvFhcvbDccThjbFBqq
SSgdHNfSHHgzLHtLNWSPQQPMQVpmmppVCmQZCW
pPssrWWLdndHPJdd
QNQFTLNBFTzzgjfGTjffFNZjCSGnHDnSDJHnDScttDCcDnmd
FVzVLZwZZgswqqrbphbR
VpWCZjCwWnppZpqnhNjjNZjFLtLzQJHdHLQRzWLRzRztHJ
DMGPmPMgTSmsgQzRFbdHRLJgdn
csDMPMGDDvMSSPnDTvrDChhwljlqNNjchNCjNVcf
WpGGmbSGpVWWpjMMTNdfCFNdFfRNwNSF
JsQztzrvrJqsTTRbbvFBhhhv
cLrDqLccsLqbDHGpZWDHgjGlZW
QGMQJMmsJmMCmmqjsRvLvvdgvgVvDVdD
BDcrcNbNppwTpzRdvvchhFvfFv
plBBwWrbpQHDjGmGJl
mzFlTdmSDzrPvCJqqDVVNC
hfRmhgjRhnfwnRHcnhGGvPJQPvvfLfQvNLGv
BhhnjMgRWghpwjRWMRjrZzdbSbsdstTrltdmMs
bLLnbqjpvplnDvNlqpqBWJZSdPJCNdJJThhSPhTd
HFwHHQMMFHGzGwRPPJPTWthTZtJSQr
mfWMHFHWHmgmFcwGwwpbDljqjBDcDnLcVnlb
wBrWBwSWRJMBwdZnPQPgFnwGVF
fLjfbsvDDfvvqqGqZGqmPQgqTGGG
vZLsjzjjZCzJWRNSBR
jTRbRHHqPqTRBHqdjhgvgghhZQdDvvgvhC
WLWWzzFszsmNFGWSFmMrpghCtZvhlQNDgQCDgctC
FJsLsSrDmsFSDLWrzJmmMsGqjRBVbJTBVPVBbBqRjPBjHn
QbwwnDDQDcDfSbDbfhhrvrCtJMvJSCvvJh
FWRjjLjmdZWdWNBFNWNlNQQrMGvvMGgssGvQRvrMJs
BjWdlBpmdmBWFWdpWfPfpVnVwfHpqPQDbq
SqrvlMldqvSWdGPTGzWpWpzpHP
tRwmhtbsRRFsLwGGTVDHppTNdbVp
FRCRQdCFtCLmBhCcmmQdhFdCvnfjffjZlZnjSnvfcSrrgMgn
GQQtNJQWWcqPPhMMtwqD
WpWLlBWZCvhjwMMZqDDP
WgvmLVmHCbpppLgdllHddvCmFGzGnfsJJQJsJncSsccFVffF
HcSsSlTTvvPPWWNMWWgPTPPbGbbrwJQbrrDphrHJJRpRhp
ztfLqqzmRwDGlLDb
fdVtmqjdZBmSvjsPSWlTgv
DPvDhhMRRMhRNDLPMNsbwHwrjgnddqddrWdPtHzr
pcBGSpcVBfJWCcmJGGwHtzgrrtwqzdrtrngG
mllBlBZmMlQWRbQv
SGZBSFMZllJWmzvfpp
NTqbNrhHNHWgNqHrNhNQbbjHJLcnfnzLLnLmfcfccJcfQLcL
HggbTNRRTHqqbVSGMSZVWDMDwVPs
SBsSlvbPlFPvRlbPsMFZLgVLrLsJVgzrCJfVCH
jcNddNdGzZrVgNVJ
tTGwdcmWGdtwQmwmwZdwSlhBPbhPTBFRhlhSMFMR
RzStzTzzvvQvSHVvhVgBqMMFqhPM
ddlLLwNVLWLjbbLrjrbWrwmlhcFmBGgFMMPgBcGBBqPhggMs
dLwdVCVWWdfNwNwLrWrbfbJNptzDDHRnHptHtznHTppnQCtR
RzcfMBHLzpDQFmnDSWNB
dbqjtjVqJZZGjPGJCPGbPndNNDglrmQmNSDgSlSSng
hjCTqhCJbhVCGNvMcfvhfRLhvchz
sDDqDMtqshJhPvhhCpSCCWlZHSWp
bffRcbBGGTwGfGfbNjgSHZSgWwplHCClZZ
RTQBbcnbRNmGbGTQLbmbJVqLllsDVMsPDVVvttMd
nbLBjnqwgfRRBgBwnllbLlwScvPdZPcScZPcdFZJPvZPvcMZ
tChQpphHrrHztssZdcDJcPZcMvWv
hpTHVMQMtQtVpzBfwjfRnfwfnjVl
ljJlvvJQlrlcJcWpPzgthnPnzMgpgSpC
smtmZBmHZTVttHmqFqmzCSZSdndzShPNgPShgP
bVqFHLqLqfHHFwbBLHcwDQrDrtjlQvGjlRQQ
pwhVsPvVVCFtmhPhzqGqqZMZvGTTTMlGWM
drrrrDfDRrNQdQdrRrBdjGWqWqWlGlGtlGbGZGBTLc
tSDfgnHrdDtVSPSshJCSPh
WlWlDqhglLhsdgrcbFdJJpPpdBbB
ZQZvSvzRMSzjZjvZmMMpbFPQFVBrVbPcpbJFLB
SwGZmjvCRSMRjMzZvRnstHftNfswHsflLhNWHf
jsprCvGRQrtjCsQrGsrzvGHhgmHVmHZgggmMGVmhMbHm
FFFdDSdwSffJWqqMzzMmDVbZ
LLcdcfcfPwwBzdTTdtvlsrjCtvPvprnsjR
MvtSqNSWMzjwzFTD
ZRPlcRpQszNgszNwVT
bcZcrcPlcPLLLZllPlbcbLSBfWCvHvWWNSmSqNqfWN
rNdZpMGnddgggwHwzRPCzDDD
vcvhcTLhZLhLPCPHPDPPVvzH
LTmBmthWBchWLttttFJFLlFnGJNsfpdjNsnMnMpnpZdssn
ZHWFCvqBDdqqqCTDHHBWrgppTMhhVpspMPQcSgQVPS
jblbGffntRwltfMQVrrQscphfg
ztJrGtbwGztbmtzzRGnRznWNNCWmHHdFHdFNWWHHqCqZ
WGWSSZvVvqmrmzPm
NgjtwFFlwDsFghNsMtlcjljcPqrQHcZzQznpQQprnqqzHQ
tgMCwNhtgbdLZRbZCT
PQSPQrSGZnGnVFhpVhRRlvLvBDRV
tjctcjTMMpDTvFTlRD
JCftsccFCcmsJJGZGGmPHnQrGwGS
TrjRFFRnpnRCHNFSjSRrffJvJfzqQBsjqQqzzffd
ZtlgMDhZhgmGDLVZLlGtLPqdQQvvfBJJqzzBPdMzdd
VlLDgLLDWtGZwgtRNTNrFTqCwqHTrr
LpcDFDMMPjMLLjpcDGCHgHssGHWnbCBWBHvm
QfZhrhVVdZThlZlfVvVzZrTbgQnBHsCCHgJBsCsJBHmBmn
wwtvfZztlTVlhtrzzlLNpFFRjMPDpRcPFwRj
VzZhhQHQJJWJSSFWDGclbmNPgglPgVGc
ddBTqCjjBCcrqrCRrwGPGmmDGmbpBGNpNNgg
CRMjwsjwsLdLRrQFJSvMFMWZcHFW
JgJJPvtrhRPQQzSRMQFFSF
BLqsjsdLsMBqblnsGbBqVqdwSQSCSWwNFwczQWCNNwNCHn
ljqbpLbbdDlbDbqDDVtMttTTgpJJgThhJrJr
nflndmjbSnlTQGwvWGPHGRGj
NtstcMcDJMvwgHfFvDgR
qqqpLrMsLLqLNNnzbrdlbZSrznfz
ttZCCFjNjnPVCFQPPFbbStrzqzqrrrcwtmJJ
gTTMRMTWsTGGTddHTTbBzBLSmqbbJGzGmqqb
HpgpMTvRhHHTRDhMsHdHDRhjJlVPJjNFJnnFpQQVfPCjnP
VqJVQPpjQqPBbHwldmLfVVmd
tMGvrzzDGCDDddwLbgLvLwcm
TWDWCzTZDGMZtzWWtsFhbRRqRQRjhbNQBBTh
zgLgLHnnzCCvnsHSsZBZBsTRdD
rslllhJjcQNNGjpWJlSRTRdwBVSSNTPVSdPB
jGrGqjJfqccrfqGcGplrJpFvzggqmCtMzmsMnvMvvCgm";
    }
}
