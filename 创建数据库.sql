CREATE DATABASE LibraryManagement;
--֧������
ALTER DATABASE LibraryManagement COLLATE Chinese_PRC_CI_AS; 
USE LibraryManagement;

CREATE TABLE [user] (
    user_id INT IDENTITY(1,1) PRIMARY KEY,
    user_name VARCHAR(50),
    user_password VARCHAR(50),
    user_permission INT
);

CREATE TABLE reader (
    reader_id INT IDENTITY(1,1) PRIMARY KEY,
    reader_name VARCHAR(50),
    reader_gender VARCHAR(10),
    reader_tel VARCHAR(20),
    user_id INT,
    FOREIGN KEY (user_id) REFERENCES [user](user_id) ON DELETE CASCADE
);

CREATE TABLE book (
    book_id INT IDENTITY(1,1) PRIMARY KEY,
    book_name VARCHAR(100),
    book_author VARCHAR(50),
    book_publisher VARCHAR(50),
    book_publish_date DATETIME,
    book_price FLOAT,
    book_stock INT
);

CREATE TABLE borrow (
    borrow_id INT IDENTITY(1,1) PRIMARY KEY,
    reader_id INT,
    book_id INT,
    borrow_date DATETIME,
    due_date DATETIME,
    return_date DATETIME,
    FOREIGN KEY (reader_id) REFERENCES reader(reader_id) ON DELETE CASCADE,
    FOREIGN KEY (book_id) REFERENCES book(book_id) ON DELETE CASCADE
);

CREATE TABLE category (
    category_id INT IDENTITY(1,1) PRIMARY KEY,
    category_name VARCHAR(50)
);

CREATE TABLE book_category (
    id INT IDENTITY(1,1) PRIMARY KEY,
    book_id INT,
    category_id INT,
    FOREIGN KEY (book_id) REFERENCES book(book_id) ON DELETE CASCADE,
    FOREIGN KEY (category_id) REFERENCES category(category_id) ON DELETE CASCADE
);


--all_books��ͼ
CREATE VIEW all_books AS 
SELECT b.*, STRING_AGG(c.category_name, ',') AS categories 
FROM book b LEFT JOIN book_category bc ON b.book_id = bc.book_id 
LEFT JOIN category c ON bc.category_id = c.category_id 
GROUP BY b.book_id, b.book_name, b.book_author, b.book_publisher, b.book_publish_date, b.book_price, b.book_stock;



-- �������� for [user]
INSERT INTO [user] (user_name, user_password, user_permission) VALUES 
('admin', 'admin', 1),
('user1', 'password1', 0),
('user2', 'password2', 0),
('user3', 'password3', 0),
('user4', 'password4', 0),
('user5', 'password5', 0);

-- �������� for reader
INSERT INTO reader (reader_name, reader_gender, reader_tel, user_id) VALUES 
('����', '��', '12345678901', 2),
('����', 'Ů', '13912345678', 3),
('����', '��', '18888888888', 4),
('����', 'Ů', '17777777777', 5),
('����', '��', '16666666666', 6);

-- �������� for book
INSERT INTO book (book_name, book_author, book_publisher, book_publish_date, book_price, book_stock) VALUES 
('����', '���°Գ�', '�������ճ�����', '2012-08-01', 29.99, 50),
('����¶�', '�����ǡ������˹', '�Ϻ����湫˾', '2011-04-01', 38.00, 20),
('����', '������', '���������', '2008-01-01', 29.99, 30),
('Χ��', 'Ǯ����', '������ѧ������', '1991-04-01', 18.00, 15),
('������������ħ��ʯ', 'J.K.����', '������ѧ������', '2000-09-01', 26.00, 25),
('ʱ���ʷ', 'ʷ�ٷҡ�����', '���Ͽ�ѧ����������', '2000-01-01', 35.00, 12),
('���������ɺ�', '������', '�������ճ�����', '2013-05-01', 28.00, 30),
('׷���ݵ���', '���յ¡�������', '�Ϻ����������', '2006-05-01', 29.80, 18);

-- �������� for borrow
INSERT INTO borrow (reader_id, book_id, borrow_date, due_date, return_date) VALUES 
(1, 2, '2021-01-01 12:32:32', '2021-02-01 09:33:44', null),
(2, 1, '2021-02-01 11:32:22', '2021-03-01 18:32:22', null),
(3, 3, '2021-03-01 02:32:32', '2021-04-01 19:22:11', null),
(4, 4, '2021-04-01 05:13:55', '2021-05-01 12:24:33', null),
(5, 5, '2021-05-01 08:22:11', '2021-06-01 09:55:44', null),
(1, 6, '2021-06-01 12:43:22', '2021-07-01 16:33:58', null),
(2, 7, '2021-07-01 09:09:09', '2021-08-01 11:11:11', null),
(3, 8, '2021-08-01 15:22:33', '2021-09-01 19:38:22', null);

-- �������� for category
INSERT INTO category (category_name) VALUES 
('����'),
('��ѧ'),
('�ƻ�'),
('����'),
('��ʷ');

-- �������� for book_category
INSERT INTO book_category (book_id, category_id) VALUES 
(1, 1),
(2, 2),
(3, 3),
(2, 3),
(4, 2),
(5, 3),
(6, 4),
(7, 2),
(8, 5),
(4, 4),
(1, 5),
(3, 1),
(5, 2),
(7, 4),
(2, 1),
(6, 2),
(8, 1);
