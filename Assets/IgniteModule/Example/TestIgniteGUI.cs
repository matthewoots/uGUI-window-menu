using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using IgniteModule;

namespace IgniteModule
{
    public class TestIgniteGUI : MonoBehaviour
    {
        private string posVector;
        private Vector3 pos;
        public Vector3 homelocation;
        private bool initialised;
        public float limit = 150;
        private Vector2 WindowSize;
        private float timeElapsed;
        public float WindowSizeX = 350;
        public float WindowSizeY = 350;
        public double logMessageFrequency = 0.1;
        public enum ExampleEnum
        {
            OptionA,
            OptionB,
            OptionC,
            OptionD
        }

        public enum HomeEnum
        {
            Base,
            Forest,
            Warehouse
        }
        IgniteWindow logWindow = null;
        

        void Start()
        {
            homelocation = new Vector3(0, 0, 0);
            WindowSize = new Vector2(WindowSizeX, WindowSizeY);
            logWindow = IgniteWindow.Create("Logger", open: true, windowSize:WindowSize).SetLeftBottomPos();
            ExampleWindow();         
        }

        private void Update()
        {
            pos = this.transform.position;
            pos -= homelocation;
            timeElapsed += Time.deltaTime;
            posVector = "Pose:      " + pos.x.ToString("0.00") + " " + pos.y.ToString("0.00") + " " + pos.z.ToString("0.00");
            if (timeElapsed > 1/logMessageFrequency)
            {
                logWindow.AddLabel(posVector);
                timeElapsed = 0;
            }
            
        }

        float sliderValue = 0f;
        void ExampleWindow()
        {
            IgniteWindow.Create("Window", windowSize:WindowSize)
                        .SetRightBottomPos()
                        .AddButton("Time", () => Empty())
                        .AddMonitoringLabel(() => System.DateTime.Now.ToString())
                        .AddButton("Vector3", () => Empty())
                        .AddMonitoringLabel(() => posVector)
                        // .AddMonitoringSlider("x",() => pos.x,-limit,limit)
                        // .AddMonitoringSlider("y",() => pos.y,-limit,limit)
                        // .AddMonitoringSlider("z",() => pos.z,-limit,limit)
                        
                        .AddButton("Clear LogWindow", () => logWindow.Clear())
                        .AddButton("ExampleButton", () => logWindow.AddLabel("Button Click"))
                        .AddLabel("ChangeHome")
                        .AddToggleGroup()
                            .LastNestedGroup
                            .AddToggle("Base", v => ChangeHome(HomeEnum.Base), defaultValue: true)
                            .AddToggle("Forest", v => ChangeHome(HomeEnum.Forest), defaultValue: false)
                            .AddToggle("Warehouse", v => ChangeHome(HomeEnum.Warehouse), defaultValue: false)
                        
                        // .AddButton("Label", "Labelbutton", () => logWindow.AddLabel("Button Click"))
                        // .AddToggle(v => logWindow.AddLabel("Toggle: " + v))
                        // .AddToggle("Label", v => logWindow.AddLabel("Toggle: " + v))
                        // .AddToggleWithButton("Button", v => logWindow.AddLabel("Button Click: " + v))
                        // .AddSlider(v => logWindow.AddLabel("Slider Value Change" + v))
                        // .AddSlider("Slider", v => logWindow.AddLabel("Slider: " + v), -100f, 100f, true)
                        // .AddFoldout("Foldout")
                        //     .LastNestedGroup
                        //     .AddLabel("Label")
                        //     .AddButton("Button", () => logWindow.AddLabel("Button Click"))
                        // .Parent
                        // .AddLabel("ToggleGroup")
                        // .AddToggleGroup()
                        //     .LastNestedGroup
                        //     .AddToggle("Toggle 1", v => logWindow.AddLabel("Toggle1: " + v), defaultValue: true)
                        //     .AddToggle("Toggle 2", v => logWindow.AddLabel("Toggle2: " + v), defaultValue: false)
                        //     .AddToggle("Toggle 3", v => logWindow.AddLabel("Toggle3: " + v), defaultValue: false)
                        // .Parent
                        // .AddDropdown(v => logWindow.AddLabel("Dropdown Value: " + v), "option A", "option B", "option C")
                        // .AddInputField(v => logWindow.AddLabel("InputField Value: " + v), v => logWindow.AddLabel("InptuField EndEdit:" + v), "Input", "PlaceHolder")
                        // .AddInputField("InputField", v => logWindow.AddLabel("InputField Value: " + v), v => logWindow.AddLabel("InptuField EndEdit:" + v), "Input", "PlaceHolder")
                        // .AddInputFieldWithButton("OpenURL", url => Application.OpenURL(url), initialValue: "https://www.youtube.com/channel/UCmgWMQkenFc72QnYkdxdoKA")
            ;
        }

        void SetWindowPos()
        {
            IgniteWindow.Create("SetPos")
                        .AddButton("LeftTop", () => IgniteWindow.Create("LeftTop").SetLeftTopPos())
                        .AddButton("LeftBottom", () => IgniteWindow.Create("LeftBottom").SetLeftBottomPos())
                        .AddButton("RightTop", () => IgniteWindow.Create("RightTop").SetRightTopPos())
                        .AddButton("RightBottom", () => IgniteWindow.Create("RightBottom").SetRightBottomPos())
                        ;
        }

        void Grid()
        {
            var grid = IgniteWindow.Create("Grid")
                                    .AddFoldout("Grid")
                                    .LastNestedGroup
                                    .AddGridGroup(new Vector2(100, 30))
                                    .LastNestedGroup;

            for (int i = 0; i < 10; ++i)
            {
                var index = i;
                grid.AddButton(i.ToString("D8"), () => Debug.Log("Grid: " + index));
            }
        }

        void ChangeHome(HomeEnum value)
        {
            switch (value)
            {
                case HomeEnum.Base:
                    homelocation = new Vector3(0,0,0);
                    break;
                case HomeEnum.Forest:
                    homelocation = new Vector3(20,-1,180);
                    break;
                case HomeEnum.Warehouse:
                    homelocation = new Vector3(100,0,100);
                    break;
                default:
                    Console.WriteLine("Error");
                    break;
            }
        }

        void Empty()
        {
        }
    }
}