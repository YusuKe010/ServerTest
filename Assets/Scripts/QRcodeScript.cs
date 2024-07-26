using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ZXing;        //QR�R�[�h�쐬�ɕK�v
using ZXing.QrCode; //QR�R�[�h�쐬�ɕK�v

namespace Server
{
    public class QRcodeScript : MonoBehaviour
    {
        [SerializeField] Image QRcodeSprite;//�ŏI�I�ɕ\������SpriteRenderer�I�u�W�F�N�g
        [SerializeField] string ImageLink = "http://*:7000";//QR�R�[�h��������URL

        private Texture2D EncodedQRTextire;//�G���R�[�h���ďo����QR�R�[�h��Txture2D������

        private int QrTxtureW = 256;//�쐬����e�N�X�`���T�C�Y
        private int QrTxtureH = 256;//�쐬����e�N�X�`���T�C�Y



        void Start()
        {
            //�V�K�̋�̃e�N�X�`�����쐬
            EncodedQRTextire = new Texture2D(QrTxtureW, QrTxtureH);

            //�G���R�[�h����
            var color32 = Encode(ImageLink, EncodedQRTextire.width, EncodedQRTextire.height);

            //https://docs.unity3d.com/2018.4/Documentation/ScriptReference/Texture2D.SetPixels32.html
            //�s�N�Z���J���[�̃u���b�N��ݒ�
            EncodedQRTextire.SetPixels32(color32);

            //https://docs.unity3d.com/ja/2017.4/ScriptReference/Texture2D.Apply.html
            //�G���R�[�h�Ŏ擾�������ŕύX��K�p����
            EncodedQRTextire.Apply();

            //�X�v���C�g���쐬���ăI�u�W�F�N�g�ɒ���t��
            QRcodeSprite.sprite = Sprite.Create(EncodedQRTextire, new Rect(0, 0, QrTxtureW, QrTxtureH), Vector2.zero);

        }




        //32 �r�b�g�`���ł� RGBA �̐F�̕\��
        //https://docs.unity3d.com/ja/2018.4/ScriptReference/Color32.html

        //�G���R�[�h�����i�����̓T���v���ʂ�j
        private static Color32[] Encode(string textForEncoding, int width, int height)
        {
            var writer = new BarcodeWriter
            {
                Format = BarcodeFormat.QR_CODE,

                Options = new QrCodeEncodingOptions
                {
                    Height = height,
                    Width = width
                }
            };
            return writer.Write(textForEncoding);
        }
    }
}
