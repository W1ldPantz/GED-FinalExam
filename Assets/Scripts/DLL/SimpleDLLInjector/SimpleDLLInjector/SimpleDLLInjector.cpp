#include <iostream>
#include <Windows.h>
#include <TlHelp32.h>

//https://www.youtube.com/watch?v=PZLhlWUmMs0&t=0s

DWORD GetProcId(const char* procName)
{
    DWORD procId = 0;
    HANDLE hSnap = CreateToolhelp32Snapshot(TH32CS_SNAPPROCESS, 0);

    //See if good snapshot
    if(hSnap != INVALID_HANDLE_VALUE)
    {
        PROCESSENTRY32 pe32;
        pe32.dwSize = sizeof(PROCESSENTRY32);
        if(Process32First(hSnap, &pe32))
        {
            do
            {
                if(!_stricmp(pe32.szExeFile, procName))
                {
                    procId = pe32.th32ProcessID;
                    break;
                }
            }
            while(Process32Next(hSnap, &pe32));
        }
    }
    CloseHandle(hSnap);
    return procId;
}


int main(int argc, char* argv[])
{
    const char* dllPath = "C:\\Users\\gabek\\Downloads\\DLL\\GameStatsDLL.dll";
    const char* procName = "Subnautica.exe";
    DWORD procId = GetProcId(procName);

    
    while(!procId)
    {
        procId = GetProcId(procName);
        Sleep(30);
    }


    HANDLE hProc = OpenProcess(PROCESS_ALL_ACCESS, FALSE, procId);

    if(hProc && hProc != INVALID_HANDLE_VALUE)
    {
        //Allocates memory in an external process, allocating max memory. for string path 
        void* location = VirtualAllocEx(hProc, 0, MAX_PATH, MEM_COMMIT | MEM_RESERVE, PAGE_EXECUTE_READWRITE);

        if(location) WriteProcessMemory(hProc, location, dllPath, strlen(dllPath), nullptr);

        HANDLE hThread = CreateRemoteThread(hProc, 0, 0, (LPTHREAD_START_ROUTINE)LoadLibraryA, location, 0, nullptr);
        std::cout << hThread;

        if(hThread) CloseHandle(hThread);
    }
    if(hProc) CloseHandle(hProc);

    std::cout << "Completed Process successfully?";
    return 0;
}
