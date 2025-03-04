#pragma once
#include "HL2ResearchMode.g.h"
#include "ResearchModeApi.h"
#include <stdio.h>
#include <iostream>
#include <sstream>
#include <wchar.h>
#include <thread>
#include <mutex>
#include <atomic>
#include <future>
#include <cmath>
#include <DirectXMath.h>
#include <vector>
#include <winrt/Windows.Foundation.h>
#include<winrt/Windows.Perception.Spatial.h>
#include<winrt/Windows.Perception.Spatial.Preview.h>
#include <shared_mutex>
#include "TimeConverter.h"

namespace winrt::HL2UnityPlugin::implementation
{
    struct HL2ResearchMode : HL2ResearchModeT<HL2ResearchMode>
    {
        HL2ResearchMode();

        UINT16 GetCenterDepth();
        int GetDepthBufferSize();
        int GetLongDepthBufferSize();
        /*hstring PrintDepthResolution();
        hstring PrintDepthExtrinsics();
		hstring PrintLFResolution();
		hstring PrintLFExtrinsics();
		hstring PrintRFResolution();
		hstring PrintRFExtrinsics();*/

        hstring GetTransformName() { return _lastTransformName;  }
        hstring GetBinaryDepthName() { return _lastBinaryDepthName; }
        hstring GetRectColorName() { return _lastRectColorName; }
        hstring GetHiColorName() { return _lastHiColorName; }
        hstring GetDepthImageName() { return _lastDepthImageName; }
        hstring GetPointCloudName() { return _lastPointCloudName; }

        hstring _lastTransformName;
        hstring _lastBinaryDepthName;
        hstring _lastRectColorName;
        hstring _lastHiColorName;
        hstring _lastDepthImageName;
        hstring _lastPointCloudName;

        void InitializeDepthSensor();
        void InitializeLongDepthSensor();
        void InitializeSpatialCamerasFront();
        winrt::Windows::Foundation::IAsyncAction InitializePVCamera();

        void StartDepthSensorLoop();
        void StartLongDepthSensorLoop();
        void StartSpatialCamerasFrontLoop();
        void StartPVCameraLoop();

        void StopAllSensorDevice();
        
        bool DepthMapTextureUpdated();
        bool ShortAbImageTextureUpdated();
        bool PointCloudUpdated();
        bool LongDepthMapTextureUpdated();

		bool LFImageUpdated();
		bool RFImageUpdated();
        bool LRImageUpdated();
        bool RRImageUpdated();

        void SetCaptureHiResColorImage() { m_bCaptureHiResImages = true; }
        bool IsCapturingHiResColor() { return m_bCaptureHiResImages; }

        void SetCaptureRectColorImage() { m_bCaptureRectColor = true; }
        bool IsCapturingRectColor() { return m_bCaptureRectColor; }

        void SetCaptureTransforms() { m_bCaptureTransform = true; }
        bool IsCapturingTransforms() { return m_bCaptureTransform; }

        void SetCaptureDepthImages() { m_bCaptureDepthImages = true; }
        bool IsCapturingDepthImages() { return m_bCaptureDepthImages; }

        void SetCaptureColoredPointCloud() { m_bCaptureColoredPointCloud = true; }
        bool IsCapturingColoredPointCloud() { return m_bCaptureColoredPointCloud; }

        void SetCaptureBinaryDepth() { m_bCaptureBinaryDepth = true; }
        bool IsCapturingBinaryDepth() { return m_bCaptureBinaryDepth; }

        void SetQRCodeDetected();

        bool IsQRCodeDetected() 
        { 
            return m_bIsQRCodeDetected; 
        }

        void SetReferenceCoordinateSystem(Windows::Perception::Spatial::SpatialCoordinateSystem refCoord);
        void SetPointCloudRoiInSpace(float centerX, float centerY, float centerZ, float boundX, float boundY, float boundZ);
        void SetQRTransform(float f00, float f01, float f02, float f03, float f10, float f11, float f12, float f13, float f20, float f21, float f22, float f23, float f30, float f31, float f32, float f33);
        void SetPointCloudDepthOffset(uint16_t offset);

        com_array<uint16_t> GetDepthMapBuffer();
        com_array<uint16_t> GetDepthMapBufferFiltered();
        com_array<uint8_t> GetDepthMapTextureBuffer();
        com_array<uint16_t> GetShortAbImageBuffer();
        com_array<uint8_t> GetShortAbImageTextureBuffer();
        com_array<uint16_t> GetLongDepthMapBuffer();
        com_array<uint8_t> GetPVColorBuffer();
        com_array<uint8_t> GetLongDepthMapTextureBuffer();
		com_array<uint8_t> GetLFCameraBuffer();
		com_array<uint8_t> GetRFCameraBuffer();
        com_array<uint8_t> GetLRCameraBuffer();
        com_array<uint8_t> GetRRCameraBuffer();

        com_array<float> GetPointCloudBuffer();
        com_array<float> GetCenterPoint();
        com_array<float> GetDepthSensorPosition();
        com_array<float> GetDepthToWorld();
        com_array<float> GetCurrRotation();
        com_array<float> GetCurrPosition();
        com_array<float> GetPVMatrix();
        com_array<float> GetLocalDepthBuffer();

        std::mutex mu;
        unsigned int _frameCount = 0;

    protected:
        void OnFrameArrived(
            const winrt::Windows::Media::Capture::Frames::MediaFrameReader& sender,
            const winrt::Windows::Media::Capture::Frames::MediaFrameArrivedEventArgs& args);
    private:

        winrt::Windows::Foundation::IAsyncAction StartColorAsync();
        winrt::event_token m_OnFrameArrivedRegistration;
        
        winrt::Windows::Media::Capture::Frames::MediaFrameReference m_latestFrame = nullptr;
        winrt::Windows::Media::Capture::Frames::MediaFrameReader m_mediaFrameReader = nullptr;

        float* m_pointCloud = nullptr;
        float* m_localDepth = nullptr;
        int m_pointcloudLength = 0;
        int m_localDepthLength = 0;
        UINT16* m_depthMapFiltered = nullptr;
        UINT16* m_depthMap = nullptr;
        UINT8* m_depthMapTexture = nullptr;
        UINT16* m_shortAbImage = nullptr;
        UINT8* m_shortAbImageTexture = nullptr;
        UINT16* m_longDepthMap = nullptr;
        UINT8* m_longDepthMapTexture = nullptr;
		UINT8* m_LFImage = nullptr;
		UINT8* m_RFImage = nullptr;
        UINT8* m_LRImage = nullptr;
        UINT8* m_RRImage = nullptr;
        UINT8* m_pixelBufferData = nullptr;

        IResearchModeSensor* m_depthSensor = nullptr;
        IResearchModeCameraSensor* m_pDepthCameraSensor = nullptr;
        IResearchModeSensor* m_longDepthSensor = nullptr;
        IResearchModeCameraSensor* m_pLongDepthCameraSensor = nullptr;
        IResearchModeSensor* m_LFSensor = nullptr;
        IResearchModeCameraSensor* m_LFCameraSensor = nullptr;
        IResearchModeSensor* m_RFSensor = nullptr;
        IResearchModeCameraSensor* m_RFCameraSensor = nullptr;
        IResearchModeSensor* m_LFSensorSide = nullptr;
        IResearchModeCameraSensor* m_LFCameraSensorSide = nullptr;
        IResearchModeSensor* m_RFSensorSide = nullptr;
        IResearchModeCameraSensor* m_RFCameraSensorSide = nullptr;
        ResearchModeSensorResolution m_depthResolution;
        ResearchModeSensorResolution m_longDepthResolution;
        ResearchModeSensorResolution m_LFResolution;
        ResearchModeSensorResolution m_RFResolution;
        ResearchModeSensorResolution m_LRResolution;
        ResearchModeSensorResolution m_RRResolution;
        IResearchModeSensorDevice* m_pSensorDevice = nullptr;
        std::vector<ResearchModeSensorDescriptor> m_sensorDescriptors;
        IResearchModeSensorDeviceConsent* m_pSensorDeviceConsent = nullptr;
        Windows::Perception::Spatial::SpatialLocator m_locator = 0;
        Windows::Perception::Spatial::SpatialCoordinateSystem m_refFrame = nullptr;

        std::atomic_int m_depthBufferSize = 0;
        std::atomic_int m_longDepthBufferSize = 0;
        std::atomic_int m_colorBufferSize = 0;
        std::atomic_int m_LFbufferSize = 0;
        std::atomic_int m_RFbufferSize = 0;
        std::atomic_int m_LRbufferSize = 0;
        std::atomic_int m_RRbufferSize = 0;

        std::atomic_uint16_t m_centerDepth = 0;

        float m_centerPoint[3]{ 0,0,0 };
        float m_depthSensorPosition[3]{ 0,0,0 };
        float m_depthToWorld[16]{ 1,0,0,0,0,1,0,0,0,0,1,0,0,0,0,1 };
        float m_currRotation[16]{ 1,0,0,0,0,1,0,0,0,0,1,0,0,0,0,1 };
        float m_currPosition[16]{ 1,0,0,0,0,1,0,0,0,0,1,0,0,0,0,1 };
        float m_PVToWorld[16]{ 1,0,0,0,0,1,0,0,0,0,1,0,0,0,0,1 };
        float m_QRTransform[16]{ 1,0,0,0,0,1,0,0,0,0,1,0,0,0,0,1 };

        std::atomic_bool m_depthSensorLoopStarted = false;
        std::atomic_bool m_PVLoopStarted = false;
        std::atomic_bool m_longDepthSensorLoopStarted = false;
        std::atomic_bool m_spatialCamerasFrontLoopStarted = false;
        std::atomic_bool m_depthMapTextureUpdated = false;
        std::atomic_bool m_shortAbImageTextureUpdated = false;
        std::atomic_bool m_longDepthMapTextureUpdated = false;
        std::atomic_bool m_pointCloudUpdated = false;
        std::atomic_bool m_useRoiFilter = false;
		std::atomic_bool m_LFImageUpdated = false;
		std::atomic_bool m_RFImageUpdated = false;
        std::atomic_bool m_LRImageUpdated = false;
        std::atomic_bool m_RRImageUpdated = false;

        //interface back from Unity

        std::atomic_bool m_bCaptureHiResImages = false;
        std::atomic_bool m_bCaptureDepthImages = false;
        std::atomic_bool m_bCaptureRectColor = false;
        std::atomic_bool m_bCaptureColoredPointCloud = false;
        std::atomic_bool m_bCaptureTransform = false;
        std::atomic_bool m_bCaptureBinaryDepth = false;
        std::atomic_bool m_bIsQRCodeDetected = false;

        float m_roiBound[3]{ 0,0,0 };
        float m_roiCenter[3]{ 0,0,0 };

        static void DepthSensorLoop(HL2ResearchMode* pHL2ResearchMode);
        static void LongDepthSensorLoop(HL2ResearchMode* pHL2ResearchMode);
        static void SpatialCamerasFrontLoop(HL2ResearchMode* pHL2ResearchMode);
        static void CamAccessOnComplete(ResearchModeSensorConsent consent);

        std::string MatrixToString(DirectX::XMFLOAT4X4 mat);

        DirectX::XMFLOAT4X4 m_depthCameraPose;
        DirectX::XMMATRIX m_depthCameraPoseInvMatrix;
        DirectX::XMFLOAT4X4 m_longDepthCameraPose;
        DirectX::XMMATRIX m_longDepthCameraPoseInvMatrix;
        DirectX::XMFLOAT4X4 m_LFCameraPose;
        DirectX::XMMATRIX m_LFCameraPoseInvMatrix;
        DirectX::XMFLOAT4X4 m_LFCameraPoseSide;
        DirectX::XMMATRIX m_LFCameraPoseInvMatrixSide;
        DirectX::XMFLOAT4X4 m_RFCameraPose;
        DirectX::XMMATRIX m_RFCameraPoseInvMatrix;
        DirectX::XMFLOAT4X4 m_RFCameraPoseSide;
        DirectX::XMMATRIX m_RFCameraPoseInvMatrixSide;
        DirectX::XMFLOAT4X4 m_PVCameraPose;
        DirectX::XMMATRIX m_PVCameraPoseInvMatrix;
        DirectX::XMFLOAT4X4 m_PV_MVP;
        DirectX::XMMATRIX m_QRMatrix;
        DirectX::XMFLOAT4X4 m_QR;

        std::thread* m_pDepthUpdateThread;
        std::thread* m_pLongDepthUpdateThread;
        std::thread* m_pSpatialCamerasFrontUpdateThread;

        winrt::Windows::Foundation::Numerics::float3* _depthPts = 0;

        TimeConverter m_converter;

        static long long checkAndConvertUnsigned(UINT64 val);

        //this is for short throw depth camera...
        struct DepthCamRoi {
            float kRowLower = 0.2;
            float kRowUpper = 0.5;
            float kColLower = 0.3;
            float kColUpper = 0.7;
            UINT16 depthNearClip = 200; // Unit: mm
            UINT16 depthFarClip = 800;
        } depthCamRoi;
        UINT16 m_depthOffset = 0;
    };
}
namespace winrt::HL2UnityPlugin::factory_implementation
{
    struct HL2ResearchMode : HL2ResearchModeT<HL2ResearchMode, implementation::HL2ResearchMode>
    {
    };
}
