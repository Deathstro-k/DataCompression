# DataCompression
Решение, позволяющее сжимать любые файлы в несколько раз.

## Проекты
### Alghoritm 
Сборка с алгоритмами и преобразованиями для сжатия данных
#### Преобразования
* Burrows-Wheeler transform (BWT)
* Move-To-Front ("Стопка книг")
#### Алгоритмы сжатия
* Run Lenght Encoding
* Huffman code
### Data 
Содержит dataset-ы и расширения для работы с данными
### Decoder
Консольное приложение, которое расшифровывает закодированные файлы
### Encoder
Консольное приложение, которое использует комбинацию преобразований и алгоритмов сжатия.



## Использование

1. Клонируйте репозиторий с помощью команды:
   ```
   git clone https://github.com/Deathstro-k/DataCompression.git
   ```
2. Перейдите в директорию с запускаемыми файлами
   ```
   cd DataCompression\exe+dataset
   ```
3. Для кодирования 
   ```
   encoder.exe infile zipfile

4. Для расшифровки
   ```
   decoder.exe zipfile decfile