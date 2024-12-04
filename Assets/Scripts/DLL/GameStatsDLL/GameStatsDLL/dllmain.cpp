#include <windows.h>
#include <gdiplus.h>
#include <fstream>
#include <string>
#include <iostream>
#include <ctime>

using namespace Gdiplus;

UINT_PTR g_timerId = 0;  // Timer ID
ULONG_PTR gdiplusToken;

HANDLE g_hThread = NULL;
bool g_running = true;

// Function declarations
void Log(const std::string& message);
extern "C" __declspec(dllexport) void CaptureScreenshot();
extern "C" __declspec(dllexport) void InitializeGDIPlus();
extern "C" __declspec(dllexport) void ShutdownGDIPlus();
CLSID GetEncoderClsid(const WCHAR* format);

VOID CALLBACK TimerProc(HWND hWnd, UINT uMsg, UINT_PTR idEvent, DWORD dwTime);
DWORD WINAPI ThreadFunc(LPVOID lpParam);

// Logging function
void Log(const std::string& message) {
    std::ofstream logFile("C:\\Users\\gabek\\Downloads\\DLL\\screenshot_log.txt", std::ios_base::app);
    if (logFile.is_open()) {
        logFile << message << std::endl;
        logFile.close();
    }
}
// Simplified function to retrieve encoder CLSID
CLSID GetEncoderClsid(const WCHAR* format) {
    UINT numEncoders = 0;
    UINT size = 0;

    // Get the number of image encoders
    GetImageEncodersSize(&numEncoders, &size);
    if (size == 0) return CLSID();

    // Allocate memory for encoders
    ImageCodecInfo* pImageCodecInfo = (ImageCodecInfo*)(malloc(size));
    GetImageEncoders(numEncoders, size, pImageCodecInfo);

    CLSID clsid;
    for (UINT i = 0; i < numEncoders; i++) {
        if (wcscmp(pImageCodecInfo[i].MimeType, format) == 0) {
            clsid = pImageCodecInfo[i].Clsid;
            free(pImageCodecInfo);
            return clsid;
        }
    }
    free(pImageCodecInfo);
    return CLSID();  // Return empty CLSID if not found
}

void CaptureScreenshot() {
    Log("Starting CaptureScreenshot.");

    // Ensure output directory exists
    const std::wstring directory = L"C:\\Users\\gabek\\Downloads\\DLL";
    if (!CreateDirectory(directory.c_str(), NULL) && GetLastError() != ERROR_ALREADY_EXISTS) {
        Log("Directory access failed.");
        return;
    }

    // Get screen dimensions
    int width = GetSystemMetrics(SM_CXSCREEN);
    int height = GetSystemMetrics(SM_CYSCREEN);
    Log("Screen dimensions: " + std::to_string(width) + "x" + std::to_string(height));

    // Set up HDCs
    HDC screenDC = GetDC(NULL);
    HDC memoryDC = CreateCompatibleDC(screenDC);
    if (!memoryDC) {
        Log("Compatible DC creation failed.");
        ReleaseDC(NULL, screenDC);
        return;
    }

    // Create compatible bitmap for the screenshot
    HBITMAP hBitmap = CreateCompatibleBitmap(screenDC, width, height);
    if (!hBitmap) {
        Log("Compatible bitmap creation failed.");
        DeleteDC(memoryDC);
        ReleaseDC(NULL, screenDC);
        return;
    }
    SelectObject(memoryDC, hBitmap);

    // Capture the screen
    if (!BitBlt(memoryDC, 0, 0, width, height, screenDC, 0, 0, SRCCOPY)) {
        Log("BitBlt failed.");
        DeleteObject(hBitmap);
        DeleteDC(memoryDC);
        ReleaseDC(NULL, screenDC);
        return;
    }

    // Create a GDI+ Bitmap from the HBITMAP
    Bitmap bitmap(hBitmap, NULL);

    // Retrieve the encoder CLSID for PNG
    CLSID pngClsid = GetEncoderClsid(L"image/png");
    if (pngClsid == CLSID()) {
        Log("Error: PNG encoder CLSID retrieval failed.");
        DeleteObject(hBitmap);
        DeleteDC(memoryDC);
        ReleaseDC(NULL, screenDC);
        return;
    }
    Log("PNG encoder CLSID retrieved successfully.");

    // Define a simplified filename
    std::wstring filename = directory + L"\\screenshot_" + std::to_wstring(std::time(0)) + L".png";

   // std::wstring filename = directory + L"\\screenshot.png";

    // Attempt to save the Bitmap
    Status saveStatus = bitmap.Save(filename.c_str(), &pngClsid, NULL);
    if (saveStatus == Ok) {
        Log("Screenshot saved successfully: " + std::string(filename.begin(), filename.end()));
    } else {
        Log("Failed to save screenshot with status code: " + std::to_string(saveStatus));
    }

    // Cleanup
    DeleteObject(hBitmap);
    DeleteDC(memoryDC);
    ReleaseDC(NULL, screenDC);

    Log("CaptureScreenshot completed.");
}

// Initialization for GDI+
void InitializeGDIPlus() {
    GdiplusStartupInput gdiplusStartupInput;
    Status status = GdiplusStartup(&gdiplusToken, &gdiplusStartupInput, NULL);
    if (status == Ok) {
        Log("GDI+ initialized successfully.");
    } else {
        Log("Failed to initialize GDI+.");
    }
}

// Shutdown for GDI+
void ShutdownGDIPlus() {
    if (gdiplusToken) {
        GdiplusShutdown(gdiplusToken);
        Log("GDI+ shutdown completed.");
    }
}

// Timer callback function
VOID CALLBACK TimerProc(HWND hWnd, UINT uMsg, UINT_PTR idEvent, DWORD dwTime) {
    Log("Timer tick: taking screenshot.");
    CaptureScreenshot();
}

// Worker thread function
DWORD WINAPI ThreadFunc(LPVOID lpParam) {
    Log("Worker thread started.");

    // Start the timer with a 60-second interval
    g_timerId = SetTimer(NULL, 0, 6000, (TIMERPROC)TimerProc); // 60 seconds
    if (g_timerId == 0) {
        Log("Failed to start timer.");
        return 1;
    }

    // Run until stopped
    while (g_running) {
        CaptureScreenshot();
        Sleep(10000);  // Small delay to reduce CPU usage
         
    }

    KillTimer(NULL, g_timerId);
    Log("Worker thread stopped.");
    return 0;
}



/*
BOOL APIENTRY DllMain(HMODULE hModule, DWORD  ul_reason_for_call, LPVOID lpReserved) {
    switch (ul_reason_for_call) {
    case DLL_PROCESS_ATTACH:
        g_running = true;
        InitializeGDIPlus();  // Initialize GDI+ once on DLL load
        g_hThread = CreateThread(NULL, 0, ThreadFunc, NULL, 0, NULL);
        if (g_hThread == NULL) {
            Log("Failed to create worker thread.");
        } else {
            Log("Worker thread created successfully.");
        }
        break;

    case DLL_PROCESS_DETACH:
        g_running = false;
        ShutdownGDIPlus();    // Shutdown GDI+

        if (g_hThread != NULL) {
            WaitForSingleObject(g_hThread, INFINITE);
            CloseHandle(g_hThread);
        }
        break;
    }
    return TRUE;
}
*/