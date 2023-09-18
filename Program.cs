using System.Configuration;
using System.Diagnostics;
using System.Runtime.CompilerServices;

using DxLibDLL;

using HakusGameEngine.common.Controller;
using HakusGameEngine.common.Function;
using HakusGameEngine.common.Model;

namespace HakusGameEngine
{
    /// <summary>
    /// �v���O�����N���X
    ///   ���C�����W�b�N��`�p
    /// </summary>
    internal static class Program
    {
        /// <summary>
        /// �ݒ�Ǎ�
        /// </summary>
        public static SettingController config = new SettingController("Haku's Game Engine");

        /// <summary>
        /// NLog���K�[
        /// </summary>
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        /// <summary>
        /// �f�o�b�O���
        /// </summary>
        private static DebugInfoModel debugInfo = new DebugInfoModel();

        /// <summary>
        /// ���C���X���b�h
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            // NLog�̐ݒ蔽�f
            NLog.LogManager.Configuration = config.GetNLogSetting();

            logger.Info("==============================  Start   ==============================");

            // �C�j�V�����C�Y����
            if (!VideoFunc.InitializeVideMode(config.GetConfig()))
            {
                // �Q�[�����C�����[�v
                while (DX.ProcessMessage() == 0)
                {

                    // �L�[���͏�ԊǗ��p
                    List<KeyBindModel> keyStatus = InputFunction.GetInputKeyAllCode(config.GetConfig(), ref debugInfo);

                    // �ꎞ�I�F�G�X�P�[�v�ŏI��
                    if (keyStatus[0].StartKey != 0 || keyStatus[1].StartKey != 0)
                    {
                        break;
                    }

                    // ��ʕ`��
                    VideoFunc.DrawScreen(config.GetConfig(), ref debugInfo);
                }

                // DX���C�u�����[���
                DX.DxLib_End();
                logger.Info("DX���C�u������������܂����B");
            }
            else
            {
                logger.Fatal("�Q�[���̋N���Ɏ��s���܂����B");
                MessageBox.Show("�Q�[���̋N���Ɏ��s���܂����B", "DirectX�������G���[", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            logger.Info("==============================   End    ==============================");
        }


    }
}