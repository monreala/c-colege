-- phpMyAdmin SQL Dump
-- version 5.2.0
-- https://www.phpmyadmin.net/
--
-- Gazdă: 127.0.0.1
-- Timp de generare: mart. 18, 2026 la 10:55 AM
-- Versiune server: 10.4.27-MariaDB
-- Versiune PHP: 8.1.12

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Bază de date: `adult_edu`
--

-- --------------------------------------------------------

--
-- Structură tabel pentru tabel `curs`
--

CREATE TABLE `curs` (
  `IdCurs` int(11) NOT NULL,
  `Denumire` varchar(100) NOT NULL,
  `Formator` varchar(100) NOT NULL,
  `Pret` decimal(10,2) NOT NULL CHECK (`Pret` > 0),
  `DurataZile` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Eliminarea datelor din tabel `curs`
--

INSERT INTO `curs` (`IdCurs`, `Denumire`, `Formator`, `Pret`, `DurataZile`) VALUES
(1, 'C# для начинающих', 'Петров В.', '1500.00', 30),
(2, 'Продвинутый Java', 'Сидоров А.', '2000.00', 45),
(3, 'Основы Баз Данных', 'Иванова Е.', '1000.00', 15),
(4, 'Веб Дизайн UI/UX', 'Ceban D.', '1200.00', 20),
(5, 'Английский B2', 'Smith J.', '800.00', 60),
(6, 'Бухгалтерия 1С', 'Popa M.', '1800.00', 40);

-- --------------------------------------------------------

--
-- Structură tabel pentru tabel `cursant`
--

CREATE TABLE `cursant` (
  `IdCursant` int(11) NOT NULL,
  `Nume` varchar(50) NOT NULL,
  `Prenume` varchar(50) NOT NULL,
  `Telefon` varchar(20) NOT NULL,
  `Email` varchar(100) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Eliminarea datelor din tabel `cursant`
--

INSERT INTO `cursant` (`IdCursant`, `Nume`, `Prenume`, `Telefon`, `Email`) VALUES
(1, 'Иванов', 'Иван', '079111222', 'ivan@mail.com'),
(2, 'Смирнова', 'Анна', '068333444', 'anna@mail.com'),
(3, 'Попов', 'Дмитрий', '079555666', 'dmitry@mail.com'),
(4, 'Ceban', 'Maria', '069777888', 'maria@mail.com'),
(5, 'Rusu', 'Andrei', '078999000', 'andrei@mail.com'),
(6, 'Volkov', 'Elena', '060123123', 'elena@mail.com'),
(7, 'sad', 'sda', '12312', 'vpod@gmail.com');

-- --------------------------------------------------------

--
-- Structură tabel pentru tabel `inscriere`
--

CREATE TABLE `inscriere` (
  `IdInscriere` int(11) NOT NULL,
  `IdCursant` int(11) NOT NULL,
  `IdCurs` int(11) NOT NULL,
  `DataInscriere` date NOT NULL,
  `StatusPlata` varchar(20) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Eliminarea datelor din tabel `inscriere`
--

INSERT INTO `inscriere` (`IdInscriere`, `IdCursant`, `IdCurs`, `DataInscriere`, `StatusPlata`) VALUES
(1, 1, 1, '2023-10-01', 'Оплачено'),
(2, 1, 3, '2023-10-05', 'Оплачено'),
(3, 2, 2, '2023-10-10', 'Не оплачено'),
(4, 3, 4, '2023-10-15', 'Оплачено'),
(5, 4, 5, '2023-10-20', 'Оплачено'),
(6, 5, 6, '2023-10-25', 'Не оплачено'),
(7, 6, 1, '2023-11-01', 'Оплачено'),
(8, 2, 5, '2023-11-05', 'Оплачено');

--
-- Indexuri pentru tabele eliminate
--

--
-- Indexuri pentru tabele `curs`
--
ALTER TABLE `curs`
  ADD PRIMARY KEY (`IdCurs`);

--
-- Indexuri pentru tabele `cursant`
--
ALTER TABLE `cursant`
  ADD PRIMARY KEY (`IdCursant`),
  ADD UNIQUE KEY `Email` (`Email`);

--
-- Indexuri pentru tabele `inscriere`
--
ALTER TABLE `inscriere`
  ADD PRIMARY KEY (`IdInscriere`),
  ADD UNIQUE KEY `IdCursant` (`IdCursant`,`IdCurs`),
  ADD KEY `IdCurs` (`IdCurs`);

--
-- AUTO_INCREMENT pentru tabele eliminate
--

--
-- AUTO_INCREMENT pentru tabele `curs`
--
ALTER TABLE `curs`
  MODIFY `IdCurs` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=11;

--
-- AUTO_INCREMENT pentru tabele `cursant`
--
ALTER TABLE `cursant`
  MODIFY `IdCursant` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=8;

--
-- AUTO_INCREMENT pentru tabele `inscriere`
--
ALTER TABLE `inscriere`
  MODIFY `IdInscriere` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=11;

--
-- Constrângeri pentru tabele eliminate
--

--
-- Constrângeri pentru tabele `inscriere`
--
ALTER TABLE `inscriere`
  ADD CONSTRAINT `inscriere_ibfk_1` FOREIGN KEY (`IdCursant`) REFERENCES `cursant` (`IdCursant`) ON DELETE CASCADE,
  ADD CONSTRAINT `inscriere_ibfk_2` FOREIGN KEY (`IdCurs`) REFERENCES `curs` (`IdCurs`) ON DELETE CASCADE;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
