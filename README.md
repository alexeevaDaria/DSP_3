# DSP_3
Исследование сигналов с помощью дискретного преобразования Фурье 
1. Обработка гармонических сигналов
а) Разработать функцию для вычисления дискретного преобразования Фурье, реализующую следующие вычисления:

	Входные данные:
-	массив данных  ,  ;
-	размерность массива данных  ;
-	номер гармоники  , для которой производятся вычисления.
	Выходные параметры для функции:
-	амплитуда косинусной составляющей  ;
-	амплитуда синусной составляющей  ;
-	амплитуда гармоники  ;
-	начальная фаза гармоники  ;
	Для вычисления    и   использовать таблицу.
В соответствии с вариантом задания сформировать тестовые сигналы (см. Таб-лицу 3). Для каждого из тестовых сигналов построить амплитудный и фазовый спек-тры.
б) Восстановить исходный сигнал по спектру:
	Сравнить сигналы
2. Обработка полигармонических сигналов
а) Сформировать полигармонический сигнал
Для сформированного сигнала вычислить амплитудный и фазовый спектр сиг-нала  , ,  ;
б)  Восстановить исходный сигнал по спектру
Сравнить исходный и восстановленный  сигналы.
в)  Восстановить исследуемый сигнал по спектру без учета начальных фаз.
Сравнить исходный и восстановленный  сигналы.
	3. Разработать программную функцию для реализации быстрого преобразова-ния Фурье. Проверить ее работоспособность при обработке полигармонических сиг-налов.
	4. Реализовать цифровую фильтрацию сигналов (НЧ-фильтр, ВЧ-фильтр, по-лосовой фильтр) на основе применения прямого и обратного преобразования Фурье и удаления ненужных спектральных составляющих. Исследовать модельные и ре-альные сигналы с помощью разработанных функций.

