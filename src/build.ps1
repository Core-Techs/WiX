properties {
	$config = 'Debug'
}

Task Default -depends Build

Task Build {
   Exec { msbuild "./CoreTechs.WiX.sln" /t:build /p:configuration=$config  }
}

Task Clean {
   Exec { msbuild "./CoreTechs.WiX.sln" /t:clean /p:configuration=$config  }
}

Task Rebuild -depends Clean,Build

Task Package -depends Build {

    # merge the dependencies into the main assembly    
    md ..\Build\$config -Force
    Exec {        
        .\packages\ilmerge.2.13.0307\ILMerge.exe /internalize /target:dll /lib:"CoreTechs.WiX\bin\$config" /out:"..\Build\$config\CoreTechs.WiX.dll" CoreTechs.WiX.dll CoreTechs.Common.dll Logos.Utility.dll        
    }        
}