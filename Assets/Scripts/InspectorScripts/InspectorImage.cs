using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Inspector
{
    public class InspectorImage : InspectorItem
    {
        Button getImageBtn;
        RawImage objImage;
        public override void Init(Transform contentArea,GameObject _selectObj)
        {
            base.Init(contentArea, _selectObj);
        }

        /// <summary>
        /// 初始化属性面板的物体
        /// </summary>
        /// <param name="contentArea"></param>
        protected override void InstantInspectorItem(Transform contentArea)
        {
            base.InstantInspectorItem(contentArea);
            GameObject imageObj = GameObject.Instantiate(inspectorPanel.imageValue, contentArea);
            imageObj.transform.Find("Title").GetComponent<Text>().text = "Image";

            getImageBtn = imageObj.GetComponentInChildren<Button>();
            objImage = selectObj.GetComponentInChildren<RawImage>();

        }

        string lastPath="";
        public override void RefreshValue()
        {
            base.RefreshValue();

            if (!string.IsNullOrEmpty(componentItem.imageUrl))
            {
                if (lastPath != componentItem.imageUrl)
                {

                    lastPath = componentItem.imageUrl;
                    UiManager.Instance.GetImageByPath(componentItem.imageUrl, (sprite) =>
                    {
                        objImage.texture = getImageBtn.GetComponent<RawImage>().texture = sprite;
                    });
                }
            }
            else
            {
                objImage.texture = getImageBtn.GetComponent<RawImage>().texture = null;
            }
        }

        /// <summary>
        /// 赋值后初始化事件，为物体赋值
        /// </summary>
        protected override void InitEvent()
        {

            base.InitEvent();
            ImageFieldChangeValue(componentItem.imageUrl);
        }
        /// <summary>
        /// 初始化命令事件
        /// </summary>
        protected override void InitCommand()
        {
            base.InitCommand();

            getImageBtn.onClick.AddListener(() =>
            {
                OpenFileName openFileName = new OpenFileName();
                openFileName.structSize = System.Runtime.InteropServices.Marshal.SizeOf(openFileName);
                openFileName.filter = "图片文件(*.jpg*.png)\0*.jpg;*.png"; 
                openFileName.file = new string(new char[256]);
                openFileName.maxFile = openFileName.file.Length;
                openFileName.fileTitle = new string(new char[64]);
                openFileName.maxFileTitle = openFileName.fileTitle.Length;
                openFileName.initialDir = Application.streamingAssetsPath.Replace('/', '\\');//默认路径
                openFileName.title = "选择图片";
                openFileName.flags = 0x00080000 | 0x00001000 | 0x00000800 | 0x00000008;

                if (LocalDialog.GetOpenFileName(openFileName))
                {
                    ComponentItemCommand(componentItem, openFileName.file, ImageFieldChangeValue);
                }
            });



        }



        private void ImageFieldChangeValue(string arg0)
        {
            EventCenter.BroadcastEvent<GameObject, string, string>(EventSendType.InspectorChange, selectObj, "ImagePath", arg0);
        }



    }
}

