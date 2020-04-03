start Assets\Data\Chunker\Chunker.exe "Assets\Data\data.txt"
xcopy /s /y "Assets\Data\enginedata.json" "Projects\Content" /i
xcopy /s /y "Assets\Data\platformdata.json" "Projects\Content" /i

xcopy /s /y "Projects\Content\enginedata.json" "Projects\Platforms\Windows\Abyss.Windows\Content" /i
xcopy /s /y "Projects\Content\platformdata.json" "Projects\Platforms\Windows\Abyss.Windows\Content" /i
xcopy /s /y "Projects\Content\Data" "Projects\Platforms\Windows\Abyss.Windows\Content\Data" /i
xcopy /s /y "Projects\Content\Graphics" "Projects\Platforms\Windows\Abyss.Windows\Content\Graphics" /i
xcopy /s /y "Projects\Content\Effects" "Projects\Platforms\Windows\Abyss.Windows\Content\Effects" /i
xcopy /s /y "Projects\Content\Sounds" "Projects\Platforms\Windows\Abyss.Windows\Content\Sounds" /i