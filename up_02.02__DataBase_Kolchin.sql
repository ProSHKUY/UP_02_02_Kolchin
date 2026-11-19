--
-- PostgreSQL database dump
--

\restrict OYhv5gGjtuamO8hdyJ8R9sYUBxzmn8fLkWiNlAhcGt6PIJj7xPqYcrTw2xi9H8K

-- Dumped from database version 18.6
-- Dumped by pg_dump version 18.6

-- Started on 2026-11-19 14:29:10

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET transaction_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- TOC entry 226 (class 1259 OID 18515)
-- Name: categories; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.categories (
    category_id integer NOT NULL,
    category_name character varying(100) NOT NULL
);


ALTER TABLE public.categories OWNER TO postgres;

--
-- TOC entry 225 (class 1259 OID 18514)
-- Name: categories_category_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.categories_category_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.categories_category_id_seq OWNER TO postgres;

--
-- TOC entry 5141 (class 0 OID 0)
-- Dependencies: 225
-- Name: categories_category_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.categories_category_id_seq OWNED BY public.categories.category_id;


--
-- TOC entry 220 (class 1259 OID 18471)
-- Name: employee_roles; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.employee_roles (
    role_id integer NOT NULL,
    role_name character varying(100) NOT NULL
);


ALTER TABLE public.employee_roles OWNER TO postgres;

--
-- TOC entry 219 (class 1259 OID 18470)
-- Name: employee_roles_role_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.employee_roles_role_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.employee_roles_role_id_seq OWNER TO postgres;

--
-- TOC entry 5142 (class 0 OID 0)
-- Dependencies: 219
-- Name: employee_roles_role_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.employee_roles_role_id_seq OWNED BY public.employee_roles.role_id;


--
-- TOC entry 222 (class 1259 OID 18482)
-- Name: employees; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.employees (
    employee_id integer NOT NULL,
    role_id integer NOT NULL,
    last_name character varying(100) NOT NULL,
    first_name character varying(100) NOT NULL,
    middle_name character varying(100),
    login character varying(100) NOT NULL,
    password character varying(255) NOT NULL
);


ALTER TABLE public.employees OWNER TO postgres;

--
-- TOC entry 221 (class 1259 OID 18481)
-- Name: employees_employee_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.employees_employee_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.employees_employee_id_seq OWNER TO postgres;

--
-- TOC entry 5143 (class 0 OID 0)
-- Dependencies: 221
-- Name: employees_employee_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.employees_employee_id_seq OWNED BY public.employees.employee_id;


--
-- TOC entry 228 (class 1259 OID 18526)
-- Name: manufacturers; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.manufacturers (
    manufacturer_id integer NOT NULL,
    manufacturer_name character varying(150) NOT NULL
);


ALTER TABLE public.manufacturers OWNER TO postgres;

--
-- TOC entry 227 (class 1259 OID 18525)
-- Name: manufacturers_manufacturer_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.manufacturers_manufacturer_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.manufacturers_manufacturer_id_seq OWNER TO postgres;

--
-- TOC entry 5144 (class 0 OID 0)
-- Dependencies: 227
-- Name: manufacturers_manufacturer_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.manufacturers_manufacturer_id_seq OWNED BY public.manufacturers.manufacturer_id;


--
-- TOC entry 239 (class 1259 OID 18661)
-- Name: order_items; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.order_items (
    order_item_id integer NOT NULL,
    order_id integer NOT NULL,
    product_article character varying(50) NOT NULL,
    quantity integer NOT NULL,
    CONSTRAINT order_items_quantity_check CHECK ((quantity > 0))
);


ALTER TABLE public.order_items OWNER TO postgres;

--
-- TOC entry 238 (class 1259 OID 18660)
-- Name: order_items_order_item_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.order_items_order_item_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.order_items_order_item_id_seq OWNER TO postgres;

--
-- TOC entry 5145 (class 0 OID 0)
-- Dependencies: 238
-- Name: order_items_order_item_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.order_items_order_item_id_seq OWNED BY public.order_items.order_item_id;


--
-- TOC entry 235 (class 1259 OID 18600)
-- Name: order_statuses; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.order_statuses (
    status_id integer NOT NULL,
    status_name character varying(100) NOT NULL
);


ALTER TABLE public.order_statuses OWNER TO postgres;

--
-- TOC entry 234 (class 1259 OID 18599)
-- Name: order_statuses_status_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.order_statuses_status_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.order_statuses_status_id_seq OWNER TO postgres;

--
-- TOC entry 5146 (class 0 OID 0)
-- Dependencies: 234
-- Name: order_statuses_status_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.order_statuses_status_id_seq OWNED BY public.order_statuses.status_id;


--
-- TOC entry 237 (class 1259 OID 18639)
-- Name: orders; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.orders (
    order_id integer NOT NULL,
    order_date date NOT NULL,
    delivery_date date,
    pickup_point_id integer NOT NULL,
    client_full_name character varying(255),
    pickup_code integer NOT NULL,
    status_id integer NOT NULL
);


ALTER TABLE public.orders OWNER TO postgres;

--
-- TOC entry 236 (class 1259 OID 18638)
-- Name: orders_order_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.orders_order_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.orders_order_id_seq OWNER TO postgres;

--
-- TOC entry 5147 (class 0 OID 0)
-- Dependencies: 236
-- Name: orders_order_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.orders_order_id_seq OWNED BY public.orders.order_id;


--
-- TOC entry 233 (class 1259 OID 18589)
-- Name: pickup_points; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.pickup_points (
    pickup_point_id integer NOT NULL,
    address character varying(255) NOT NULL
);


ALTER TABLE public.pickup_points OWNER TO postgres;

--
-- TOC entry 232 (class 1259 OID 18588)
-- Name: pickup_points_pickup_point_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.pickup_points_pickup_point_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.pickup_points_pickup_point_id_seq OWNER TO postgres;

--
-- TOC entry 5148 (class 0 OID 0)
-- Dependencies: 232
-- Name: pickup_points_pickup_point_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.pickup_points_pickup_point_id_seq OWNED BY public.pickup_points.pickup_point_id;


--
-- TOC entry 231 (class 1259 OID 18547)
-- Name: products; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.products (
    article character varying(50) NOT NULL,
    product_name character varying(255) NOT NULL,
    unit_id integer NOT NULL,
    price numeric(10,2) NOT NULL,
    supplier_id integer NOT NULL,
    manufacturer_id integer NOT NULL,
    category_id integer NOT NULL,
    discount numeric(5,2) DEFAULT 0 NOT NULL,
    stock_quantity integer DEFAULT 0 NOT NULL,
    description character varying(1000),
    photo character varying(255),
    CONSTRAINT products_discount_check CHECK (((discount >= (0)::numeric) AND (discount <= (100)::numeric))),
    CONSTRAINT products_price_check CHECK ((price >= (0)::numeric)),
    CONSTRAINT products_stock_quantity_check CHECK ((stock_quantity >= 0))
);


ALTER TABLE public.products OWNER TO postgres;

--
-- TOC entry 230 (class 1259 OID 18537)
-- Name: suppliers; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.suppliers (
    supplier_id integer NOT NULL,
    supplier_name character varying(150) NOT NULL
);


ALTER TABLE public.suppliers OWNER TO postgres;

--
-- TOC entry 229 (class 1259 OID 18536)
-- Name: suppliers_supplier_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.suppliers_supplier_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.suppliers_supplier_id_seq OWNER TO postgres;

--
-- TOC entry 5149 (class 0 OID 0)
-- Dependencies: 229
-- Name: suppliers_supplier_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.suppliers_supplier_id_seq OWNED BY public.suppliers.supplier_id;


--
-- TOC entry 224 (class 1259 OID 18504)
-- Name: units; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.units (
    unit_id integer NOT NULL,
    unit_name character varying(50) NOT NULL
);


ALTER TABLE public.units OWNER TO postgres;

--
-- TOC entry 223 (class 1259 OID 18503)
-- Name: units_unit_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.units_unit_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.units_unit_id_seq OWNER TO postgres;

--
-- TOC entry 5150 (class 0 OID 0)
-- Dependencies: 223
-- Name: units_unit_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.units_unit_id_seq OWNED BY public.units.unit_id;


--
-- TOC entry 4908 (class 2604 OID 18518)
-- Name: categories category_id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.categories ALTER COLUMN category_id SET DEFAULT nextval('public.categories_category_id_seq'::regclass);


--
-- TOC entry 4905 (class 2604 OID 18474)
-- Name: employee_roles role_id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.employee_roles ALTER COLUMN role_id SET DEFAULT nextval('public.employee_roles_role_id_seq'::regclass);


--
-- TOC entry 4906 (class 2604 OID 18485)
-- Name: employees employee_id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.employees ALTER COLUMN employee_id SET DEFAULT nextval('public.employees_employee_id_seq'::regclass);


--
-- TOC entry 4909 (class 2604 OID 18529)
-- Name: manufacturers manufacturer_id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.manufacturers ALTER COLUMN manufacturer_id SET DEFAULT nextval('public.manufacturers_manufacturer_id_seq'::regclass);


--
-- TOC entry 4916 (class 2604 OID 18664)
-- Name: order_items order_item_id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.order_items ALTER COLUMN order_item_id SET DEFAULT nextval('public.order_items_order_item_id_seq'::regclass);


--
-- TOC entry 4914 (class 2604 OID 18603)
-- Name: order_statuses status_id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.order_statuses ALTER COLUMN status_id SET DEFAULT nextval('public.order_statuses_status_id_seq'::regclass);


--
-- TOC entry 4915 (class 2604 OID 18642)
-- Name: orders order_id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.orders ALTER COLUMN order_id SET DEFAULT nextval('public.orders_order_id_seq'::regclass);


--
-- TOC entry 4913 (class 2604 OID 18592)
-- Name: pickup_points pickup_point_id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.pickup_points ALTER COLUMN pickup_point_id SET DEFAULT nextval('public.pickup_points_pickup_point_id_seq'::regclass);


--
-- TOC entry 4910 (class 2604 OID 18540)
-- Name: suppliers supplier_id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.suppliers ALTER COLUMN supplier_id SET DEFAULT nextval('public.suppliers_supplier_id_seq'::regclass);


--
-- TOC entry 4907 (class 2604 OID 18507)
-- Name: units unit_id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.units ALTER COLUMN unit_id SET DEFAULT nextval('public.units_unit_id_seq'::regclass);


--
-- TOC entry 5122 (class 0 OID 18515)
-- Dependencies: 226
-- Data for Name: categories; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.categories VALUES (1, 'Художественная литература');
INSERT INTO public.categories VALUES (2, 'Учебник для вузов');
INSERT INTO public.categories VALUES (3, 'Хрестоматия');
INSERT INTO public.categories VALUES (4, 'Учебное пособие');


--
-- TOC entry 5116 (class 0 OID 18471)
-- Dependencies: 220
-- Data for Name: employee_roles; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.employee_roles VALUES (1, 'Администратор');
INSERT INTO public.employee_roles VALUES (2, 'Менеджер');
INSERT INTO public.employee_roles VALUES (3, 'Авторизированный клиент');


--
-- TOC entry 5118 (class 0 OID 18482)
-- Dependencies: 222
-- Data for Name: employees; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.employees VALUES (1, 1, 'Никифорова', 'Анна', 'Семеновна', '94d5ous@gmail.com', 'uzWC67');
INSERT INTO public.employees VALUES (2, 1, 'Стелина', 'Евгения', 'Петровна', 'uth4iz@mail.com', '2L6KZG');
INSERT INTO public.employees VALUES (3, 1, 'Михайлюк', 'Анна', 'Вячеславовна', '5d4zbu@tutanota.com', 'rwVDh9');
INSERT INTO public.employees VALUES (4, 2, 'Ситдикова', 'Елена', 'Анатольевна', 'ptec8ym@yahoo.com', 'LdNyos');
INSERT INTO public.employees VALUES (5, 2, 'Ворсин', 'Петр', 'Евгеньевич', '1qz4kw@mail.com', 'gynQMT');
INSERT INTO public.employees VALUES (6, 2, 'Старикова', 'Елена', 'Павловна', '4np6se@mail.com', 'AtnDjr');
INSERT INTO public.employees VALUES (7, 3, 'Никифорова', 'Весения', 'Николаевна', 'yzls62@outlook.com', 'JlFRCZ');
INSERT INTO public.employees VALUES (8, 3, 'Сазонов', 'Руслан', 'Германович', '1diph5e@tutanota.com', '8ntwUp');
INSERT INTO public.employees VALUES (9, 3, 'Одинцов', 'Серафим', 'Артёмович', 'tjde7c@yahoo.com', 'YOyhfR');
INSERT INTO public.employees VALUES (10, 3, 'Степанов', 'Михаил', 'Артёмович', 'wpmrc3do@tutanota.com', 'RSbvHv');


--
-- TOC entry 5124 (class 0 OID 18526)
-- Dependencies: 228
-- Data for Name: manufacturers; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.manufacturers VALUES (1, 'Яуза');
INSERT INTO public.manufacturers VALUES (2, 'Т8 Издательские технологии');
INSERT INTO public.manufacturers VALUES (3, 'Прогресс книга');
INSERT INTO public.manufacturers VALUES (4, 'Время');
INSERT INTO public.manufacturers VALUES (5, 'Лениздат');
INSERT INTO public.manufacturers VALUES (6, 'Неолит');
INSERT INTO public.manufacturers VALUES (7, 'Амрита-Русь');
INSERT INTO public.manufacturers VALUES (8, 'Златоуст');
INSERT INTO public.manufacturers VALUES (9, 'Аспект Пресс');
INSERT INTO public.manufacturers VALUES (10, 'ВКН');


--
-- TOC entry 5135 (class 0 OID 18661)
-- Dependencies: 239
-- Data for Name: order_items; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.order_items VALUES (1, 1, 'А112Т4', 2);
INSERT INTO public.order_items VALUES (2, 1, 'G843H5', 2);
INSERT INTO public.order_items VALUES (3, 2, 'G843H5', 1);
INSERT INTO public.order_items VALUES (4, 2, 'А112Т4', 1);
INSERT INTO public.order_items VALUES (5, 3, 'D325D4', 10);
INSERT INTO public.order_items VALUES (6, 3, 'S432T5', 10);
INSERT INTO public.order_items VALUES (7, 4, 'F325D4', 5);
INSERT INTO public.order_items VALUES (8, 4, 'D325D4', 4);
INSERT INTO public.order_items VALUES (9, 5, 'G432G6', 20);
INSERT INTO public.order_items VALUES (10, 5, 'H542F5', 20);
INSERT INTO public.order_items VALUES (11, 6, 'А112Т4', 2);
INSERT INTO public.order_items VALUES (12, 6, 'G843H5', 2);
INSERT INTO public.order_items VALUES (13, 7, 'C346F5', 3);
INSERT INTO public.order_items VALUES (14, 7, 'F256G6', 3);
INSERT INTO public.order_items VALUES (15, 8, 'F325D4', 1);
INSERT INTO public.order_items VALUES (16, 8, 'G432G6', 1);
INSERT INTO public.order_items VALUES (17, 9, 'J532V5', 5);
INSERT INTO public.order_items VALUES (18, 9, 'F256G6', 1);
INSERT INTO public.order_items VALUES (34, 22, 'sss1', 1);


--
-- TOC entry 5131 (class 0 OID 18600)
-- Dependencies: 235
-- Data for Name: order_statuses; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.order_statuses VALUES (1, 'Завершен');
INSERT INTO public.order_statuses VALUES (2, 'Новый ');
INSERT INTO public.order_statuses VALUES (3, 'В работе');


--
-- TOC entry 5133 (class 0 OID 18639)
-- Dependencies: 237
-- Data for Name: orders; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.orders VALUES (1, '2024-02-27', '2024-04-20', 1, 'Степанов Михаил Артёмович', 901, 1);
INSERT INTO public.orders VALUES (2, '2023-09-28', '2024-04-21', 11, 'Никифорова Весения Николаевна', 902, 1);
INSERT INTO public.orders VALUES (3, '2024-03-21', '2024-04-22', 2, 'Сазонов Руслан Германович', 903, 1);
INSERT INTO public.orders VALUES (4, '2024-02-20', '2024-04-23', 11, 'Одинцов Серафим Артёмович', 904, 1);
INSERT INTO public.orders VALUES (5, '2024-03-17', '2024-04-24', 2, 'Степанов Михаил Артёмович', 905, 1);
INSERT INTO public.orders VALUES (6, '2024-03-01', '2024-04-25', 15, 'Никифорова Весения Николаевна', 906, 1);
INSERT INTO public.orders VALUES (7, '2024-02-29', '2024-04-26', 3, 'Сазонов Руслан Германович', 907, 1);
INSERT INTO public.orders VALUES (8, '2024-03-31', '2024-04-27', 19, 'Одинцов Серафим Артёмович', 908, 2);
INSERT INTO public.orders VALUES (9, '2024-04-02', '2024-04-28', 5, 'Степанов Михаил Артёмович', 909, 2);
INSERT INTO public.orders VALUES (22, '2026-11-18', '2026-11-21', 8, NULL, 910, 2);


--
-- TOC entry 5129 (class 0 OID 18589)
-- Dependencies: 233
-- Data for Name: pickup_points; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.pickup_points VALUES (1, '420151, г. Лесной, ул. Вишневая, 32');
INSERT INTO public.pickup_points VALUES (2, '125061, г. Лесной, ул. Подгорная, 8');
INSERT INTO public.pickup_points VALUES (3, '630370, г. Лесной, ул. Шоссейная, 24');
INSERT INTO public.pickup_points VALUES (4, '400562, г. Лесной, ул. Зеленая, 32');
INSERT INTO public.pickup_points VALUES (5, '614510, г. Лесной, ул. Маяковского, 47');
INSERT INTO public.pickup_points VALUES (6, '410542, г. Лесной, ул. Светлая, 46');
INSERT INTO public.pickup_points VALUES (7, '620839, г. Лесной, ул. Цветочная, 8');
INSERT INTO public.pickup_points VALUES (8, '443890, г. Лесной, ул. Коммунистическая, 1');
INSERT INTO public.pickup_points VALUES (9, '603379, г. Лесной, ул. Спортивная, 46');
INSERT INTO public.pickup_points VALUES (10, '603721, г. Лесной, ул. Гоголя, 41');
INSERT INTO public.pickup_points VALUES (11, '410172, г. Лесной, ул. Северная, 13');
INSERT INTO public.pickup_points VALUES (12, '614611, г. Лесной, ул. Молодежная, 50');
INSERT INTO public.pickup_points VALUES (13, '454311, г.Лесной, ул. Новая, 19');
INSERT INTO public.pickup_points VALUES (14, '660007, г.Лесной, ул. Октябрьская, 19');
INSERT INTO public.pickup_points VALUES (15, '603036, г. Лесной, ул. Садовая, 4');
INSERT INTO public.pickup_points VALUES (16, '394060, г.Лесной, ул. Фрунзе, 43');
INSERT INTO public.pickup_points VALUES (17, '410661, г. Лесной, ул. Школьная, 50');
INSERT INTO public.pickup_points VALUES (18, '625590, г. Лесной, ул. Коммунистическая, 20');
INSERT INTO public.pickup_points VALUES (19, '625683, г. Лесной, ул. 8 Марта');
INSERT INTO public.pickup_points VALUES (20, '450983, г.Лесной, ул. Комсомольская, 26');
INSERT INTO public.pickup_points VALUES (21, '394782, г. Лесной, ул. Чехова, 3');
INSERT INTO public.pickup_points VALUES (22, '603002, г. Лесной, ул. Дзержинского, 28');
INSERT INTO public.pickup_points VALUES (23, '450558, г. Лесной, ул. Набережная, 30');
INSERT INTO public.pickup_points VALUES (24, '344288, г. Лесной, ул. Чехова, 1');
INSERT INTO public.pickup_points VALUES (25, '614164, г.Лесной,  ул. Степная, 30');
INSERT INTO public.pickup_points VALUES (26, '394242, г. Лесной, ул. Коммунистическая, 43');
INSERT INTO public.pickup_points VALUES (27, '660540, г. Лесной, ул. Солнечная, 25');
INSERT INTO public.pickup_points VALUES (28, '125837, г. Лесной, ул. Шоссейная, 40');
INSERT INTO public.pickup_points VALUES (29, '125703, г. Лесной, ул. Партизанская, 49');
INSERT INTO public.pickup_points VALUES (30, '625283, г. Лесной, ул. Победы, 46');
INSERT INTO public.pickup_points VALUES (31, '614753, г. Лесной, ул. Полевая, 35');
INSERT INTO public.pickup_points VALUES (32, '426030, г. Лесной, ул. Маяковского, 44');
INSERT INTO public.pickup_points VALUES (33, '450375, г. Лесной ул. Клубная, 44');
INSERT INTO public.pickup_points VALUES (34, '625560, г. Лесной, ул. Некрасова, 12');
INSERT INTO public.pickup_points VALUES (35, '630201, г. Лесной, ул. Комсомольская, 17');
INSERT INTO public.pickup_points VALUES (36, '190949, г. Лесной, ул. Мичурина, 26');


--
-- TOC entry 5127 (class 0 OID 18547)
-- Dependencies: 231
-- Data for Name: products; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.products VALUES ('А112Т4', 'Прокляты и убиты', 1, 585.00, 1, 1, 1, 25.00, 6, 'Роман-эпопею "Прокляты и убиты" Виктора Астафьева по праву считают одним из самых сильных и пронзительных произведений отечественной военной прозы.', '1.jpg');
INSERT INTO public.products VALUES ('G843H5', 'Тайны и загадки отца БраунаТайны и загадки отца Брауна', 1, 193.00, 2, 1, 1, 30.00, 9, 'Гилберт Кит Честертон — признанный классик английской литературы, один из самых ярких писателей первой половины XX века. Классикой стали его романы и многочисленные эссе, однако любовь массового читателя принесли ему рассказы об отце Брауне, тихом, застенчивом священнике, мастерски раскрывающем наиболее запутанные загадки и преступления.', '2.jpg');
INSERT INTO public.products VALUES ('D325D4', 'Девайс', 1, 1599.00, 3, 2, 1, 5.00, 12, 'Молодой фрилансер Захар Скаро устраивается на очередную подработку. Задача, казалось бы, тривиальная: тестирование нового устройства. Вот только вопрос в том, тестированием какой реальности занимается этот новый Девайс?', '3.jpg');
INSERT INTO public.products VALUES ('S432T5', 'Необыкновенное обыкновенное чудо. Школьные истории', 1, 549.00, 4, 2, 1, 15.00, 15, NULL, '4.jpg');
INSERT INTO public.products VALUES ('F325D4', 'Чук и Гек', 1, 209.00, 5, 2, 1, 18.00, 3, 'В книгу вошли повести и рассказы Аркадия Петровича Гайдара: "Чук и Гек", "Горячий камень" и "Сказка о военной тайне, о Мальчише-Кибальчише и его твердом слове"', '5.jpg');
INSERT INTO public.products VALUES ('G432G6', 'Информационная безопасность. Национальные стандарты Российской Федерации. 3-е издание. Учебное пособие', 1, 3899.00, 6, 3, 2, 22.00, 3, 'В учебном пособии рассмотрено более 300 действующих открытых документов национальной системы стандартизации Российской Федерации, включая международные и межгосударственные стандарты в области информационной безопасности по состоянию на начало 2023 года.', '6.jpg');
INSERT INTO public.products VALUES ('H542F5', 'Linux. Командная строка. Лучшие практики', 1, 1799.00, 7, 3, 2, 4.00, 5, 'Перейдите на новый уровень работы в Linux! Если вы системный администратор, разработчик программного обеспечения, SRE-инженер или пользователь Linux, книга поможет вам работать быстрее, элегантнее и эффективнее.', '7.jpg');
INSERT INTO public.products VALUES ('C346F5', 'Квантовые миры и возникновение пространства-времени', 1, 1349.00, 8, 3, 2, 5.00, 4, 'Шон Кэрролл — физик-теоретик и один из самых известных в мире популяризаторов науки — заставляет нас по-новому взглянуть на физику. Столкновение с главной загадкой квантовой механики полностью поменяет наши представления о пространстве и времени.', '8.jpg');
INSERT INTO public.products VALUES ('F256G6', 'Вселенная. Происхождение жизни, смысл нашего существования и огромный космос', 1, 1799.00, 8, 3, 2, 6.00, 2, 'Знаменитый физик Шон Кэрролл в свойственной ему увлекательной манере объясняет принципы, которые лежат в основах научных революций от Дарвина до Эйнштейна, и показывает как невероятные научные открытия последнего столетия изменили наш мир.', NULL);
INSERT INTO public.products VALUES ('J532V5', 'Пушкин. Бродский. Империя и судьба. В 2-х томах (комплект из 2-х книг)', 1, 529.00, 9, 4, 3, 8.00, 6, 'Первая книга двухтомника «Пушкин. Бродский. Империя и судьба» пронизана пушкинской темой. Пушкин — «певец империи и свободы» — присутствует даже там, где он впрямую не упоминается, ибо его судьба, как и судьба других героев книги, органично связана с трагедией великой империи.', '10.jpg');
INSERT INTO public.products VALUES ('G643F4', 'Иосиф Бродский. Избранные эссе (комплект из 6-ти книг)', 1, 4925.00, 10, 5, 3, 2.00, 24, 'Шесть сборников избранных эссе Иосифа Бродского (1940-1996), великого поэта, драматурга, мыслителя, лауреата Нобелевской премии по литературе (1987): «Будущее или далекое прошлое», «Верь своей боли», «Как читать книгу», «О русской литературе», «О тирании», «Путеводитель по переименованному городу». Все тексты представлены на английском языке и в переводе на русский и открывают автора не только как поэта, но как историка, критика, и глубокого и ироничного мыслителя.', '11.jpg');
INSERT INTO public.products VALUES ('J326V5', 'Тысячелетие императорской керамикиv', 1, 2599.00, 11, 5, 3, 5.00, 4, 'Фарфор стал величайшим символом китайской культуры. Это одно из выдающихся изобретений, внесших неоценимый вклад в мировую цивилизацию.', '12.jpg');
INSERT INTO public.products VALUES ('J632F6', 'Вечные спутники: Портреты из всемирной литературы', 1, 1599.00, 12, 5, 3, 0.00, 6, 'Книга "Вечные спутники" - это цикл критических очерков о культуре и великих литераторах, сопровождавших жизнь и творчество русского писателя, поэта, литературного критика и общественного деятеля Дмитрия Мережковского (1865–1941).', '13.jpg');
INSERT INTO public.products VALUES ('G632H6', 'Формирование литературной репутации Н.Г.Чернышевского в ХIX-XXI веках', 1, 1349.00, 13, 6, 3, 2.00, 8, 'Монография Д. А. Щербакова - новаторская. Поэтапно рассмотрены не только многочисленные суждения известных отечественных и зарубежных критиков, литературоведов, философов и политиков, различным образом характеризовавших Н. Г. Чернышевского в связи и вне связи со знаменитым романом "Что делать?', '14.jpg');
INSERT INTO public.products VALUES ('M642E5', 'Теория искусства. Краткий путеводитель', 1, 879.00, 14, 6, 3, 3.00, 2, NULL, '15.jpg');
INSERT INTO public.products VALUES ('G543F5', 'Религиозные верования с древнейших времен до наших дней', 1, 879.00, 13, 7, 3, 4.00, 6, 'Настоящее издание представляет собой сборник переводов лекций по истории дохристианских и нехристианских религий, прочитанных в Лондоне в период с 1888 по 1891 гг. авторитетными исследователями данного раздела религиоведения.', '16.jpg');
INSERT INTO public.products VALUES ('B653G6', 'Русский язык: Первые шаги. Часть 3. Учебное пособие', 1, 2699.00, 15, 8, 4, 8.00, 9, 'Пособие является завершающей частью учебного комплекса. Третья часть содержит 10 уроков (21-30, последний-повторительный). Усвоение лексико-грамматического материала рассчитано примерно на 200-240 часов аудиторных занятий.', '17.jpg');
INSERT INTO public.products VALUES ('J735J7', 'Синтетический образ индивидуального психического мира', 1, 1099.00, 16, 8, 3, 9.00, 4, 'Психика подобна определенным объектам, это фиксируют сами люди в языке и искусстве. В данном исследовании рассматриваются в этом плане образы сосуда, воронки, дерева и крепости.', '18.jpg');
INSERT INTO public.products VALUES ('H436H7', 'Английский язык в спорте: Учебное пособие', 1, 1999.00, 17, 9, 4, 2.00, 0, 'Учебное пособие подготовлено для слушателей, изу чающих английский язык как язык специальности', '19.jpg');
INSERT INTO public.products VALUES ('H475R5', 'Лексика и грамматика современного китайского языка (к тому II учебника «Новый практический курс китайского языка» под редакцией Лю Сюня): учебное пособие', 1, 608.00, 18, 10, 4, 25.00, 12, 'Пособие выступает дополнением ко второму тому учебника «Новый практический курс китайского языка» (под редакцией Лю Сюня).', '20.jpg');
INSERT INTO public.products VALUES ('sss1', 'Самый лучший папа', 1, 5000.00, 8, 6, 3, 0.00, 1, 'просто крутая книжка', 'sss1_20261117094715.jpeg');


--
-- TOC entry 5126 (class 0 OID 18537)
-- Dependencies: 230
-- Data for Name: suppliers; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.suppliers VALUES (1, 'Виктор Астафьев');
INSERT INTO public.suppliers VALUES (2, 'Гилберт Кит Честертон');
INSERT INTO public.suppliers VALUES (3, 'Кирилл Каланджи');
INSERT INTO public.suppliers VALUES (4, 'Людмила Улицкая');
INSERT INTO public.suppliers VALUES (5, 'Аркадий Гайдар');
INSERT INTO public.suppliers VALUES (6, 'Юрий Родичев');
INSERT INTO public.suppliers VALUES (7, 'Дэниел Джей Барретт');
INSERT INTO public.suppliers VALUES (8, 'Шон Кэрролл');
INSERT INTO public.suppliers VALUES (9, 'Яков Гордин');
INSERT INTO public.suppliers VALUES (10, 'Иосиф Бродский');
INSERT INTO public.suppliers VALUES (11, 'Янь Чуннянь Янь Чуннянь');
INSERT INTO public.suppliers VALUES (12, 'Дмитрий Мережковский');
INSERT INTO public.suppliers VALUES (13, 'Дмитрий Щербаков');
INSERT INTO public.suppliers VALUES (14, 'Роджер Осборн, Дэн Стерджис');
INSERT INTO public.suppliers VALUES (15, 'Любовь Беликова, Инна Ерофеева, Татьяна Шутова');
INSERT INTO public.suppliers VALUES (16, 'Сергей Моргачев');
INSERT INTO public.suppliers VALUES (17, 'Екатерина Габарта, Ирина Игнатьева');
INSERT INTO public.suppliers VALUES (18, 'Татьяна Лопаткина, Софья Маннапова');


--
-- TOC entry 5120 (class 0 OID 18504)
-- Dependencies: 224
-- Data for Name: units; Type: TABLE DATA; Schema: public; Owner: postgres
--

INSERT INTO public.units VALUES (1, 'шт.');


--
-- TOC entry 5151 (class 0 OID 0)
-- Dependencies: 225
-- Name: categories_category_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.categories_category_id_seq', 4, true);


--
-- TOC entry 5152 (class 0 OID 0)
-- Dependencies: 219
-- Name: employee_roles_role_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.employee_roles_role_id_seq', 3, true);


--
-- TOC entry 5153 (class 0 OID 0)
-- Dependencies: 221
-- Name: employees_employee_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.employees_employee_id_seq', 10, true);


--
-- TOC entry 5154 (class 0 OID 0)
-- Dependencies: 227
-- Name: manufacturers_manufacturer_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.manufacturers_manufacturer_id_seq', 10, true);


--
-- TOC entry 5155 (class 0 OID 0)
-- Dependencies: 238
-- Name: order_items_order_item_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.order_items_order_item_id_seq', 34, true);


--
-- TOC entry 5156 (class 0 OID 0)
-- Dependencies: 234
-- Name: order_statuses_status_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.order_statuses_status_id_seq', 3, true);


--
-- TOC entry 5157 (class 0 OID 0)
-- Dependencies: 236
-- Name: orders_order_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.orders_order_id_seq', 22, true);


--
-- TOC entry 5158 (class 0 OID 0)
-- Dependencies: 232
-- Name: pickup_points_pickup_point_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.pickup_points_pickup_point_id_seq', 36, true);


--
-- TOC entry 5159 (class 0 OID 0)
-- Dependencies: 229
-- Name: suppliers_supplier_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.suppliers_supplier_id_seq', 18, true);


--
-- TOC entry 5160 (class 0 OID 0)
-- Dependencies: 223
-- Name: units_unit_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.units_unit_id_seq', 1, true);


--
-- TOC entry 4934 (class 2606 OID 18524)
-- Name: categories categories_category_name_key; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.categories
    ADD CONSTRAINT categories_category_name_key UNIQUE (category_name);


--
-- TOC entry 4936 (class 2606 OID 18522)
-- Name: categories categories_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.categories
    ADD CONSTRAINT categories_pkey PRIMARY KEY (category_id);


--
-- TOC entry 4922 (class 2606 OID 18478)
-- Name: employee_roles employee_roles_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.employee_roles
    ADD CONSTRAINT employee_roles_pkey PRIMARY KEY (role_id);


--
-- TOC entry 4924 (class 2606 OID 18480)
-- Name: employee_roles employee_roles_role_name_key; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.employee_roles
    ADD CONSTRAINT employee_roles_role_name_key UNIQUE (role_name);


--
-- TOC entry 4926 (class 2606 OID 18497)
-- Name: employees employees_login_key; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.employees
    ADD CONSTRAINT employees_login_key UNIQUE (login);


--
-- TOC entry 4928 (class 2606 OID 18495)
-- Name: employees employees_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.employees
    ADD CONSTRAINT employees_pkey PRIMARY KEY (employee_id);


--
-- TOC entry 4938 (class 2606 OID 18535)
-- Name: manufacturers manufacturers_manufacturer_name_key; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.manufacturers
    ADD CONSTRAINT manufacturers_manufacturer_name_key UNIQUE (manufacturer_name);


--
-- TOC entry 4940 (class 2606 OID 18533)
-- Name: manufacturers manufacturers_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.manufacturers
    ADD CONSTRAINT manufacturers_pkey PRIMARY KEY (manufacturer_id);


--
-- TOC entry 4958 (class 2606 OID 18671)
-- Name: order_items order_items_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.order_items
    ADD CONSTRAINT order_items_pkey PRIMARY KEY (order_item_id);


--
-- TOC entry 4952 (class 2606 OID 18607)
-- Name: order_statuses order_statuses_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.order_statuses
    ADD CONSTRAINT order_statuses_pkey PRIMARY KEY (status_id);


--
-- TOC entry 4954 (class 2606 OID 18609)
-- Name: order_statuses order_statuses_status_name_key; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.order_statuses
    ADD CONSTRAINT order_statuses_status_name_key UNIQUE (status_name);


--
-- TOC entry 4956 (class 2606 OID 18649)
-- Name: orders orders_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.orders
    ADD CONSTRAINT orders_pkey PRIMARY KEY (order_id);


--
-- TOC entry 4948 (class 2606 OID 18598)
-- Name: pickup_points pickup_points_address_key; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.pickup_points
    ADD CONSTRAINT pickup_points_address_key UNIQUE (address);


--
-- TOC entry 4950 (class 2606 OID 18596)
-- Name: pickup_points pickup_points_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.pickup_points
    ADD CONSTRAINT pickup_points_pkey PRIMARY KEY (pickup_point_id);


--
-- TOC entry 4946 (class 2606 OID 18567)
-- Name: products products_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.products
    ADD CONSTRAINT products_pkey PRIMARY KEY (article);


--
-- TOC entry 4942 (class 2606 OID 18544)
-- Name: suppliers suppliers_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.suppliers
    ADD CONSTRAINT suppliers_pkey PRIMARY KEY (supplier_id);


--
-- TOC entry 4944 (class 2606 OID 18546)
-- Name: suppliers suppliers_supplier_name_key; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.suppliers
    ADD CONSTRAINT suppliers_supplier_name_key UNIQUE (supplier_name);


--
-- TOC entry 4930 (class 2606 OID 18511)
-- Name: units units_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.units
    ADD CONSTRAINT units_pkey PRIMARY KEY (unit_id);


--
-- TOC entry 4932 (class 2606 OID 18513)
-- Name: units units_unit_name_key; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.units
    ADD CONSTRAINT units_unit_name_key UNIQUE (unit_name);


--
-- TOC entry 4959 (class 2606 OID 18498)
-- Name: employees fk_employees_role; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.employees
    ADD CONSTRAINT fk_employees_role FOREIGN KEY (role_id) REFERENCES public.employee_roles(role_id) ON UPDATE CASCADE ON DELETE RESTRICT;


--
-- TOC entry 4966 (class 2606 OID 18672)
-- Name: order_items fk_order_items_order; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.order_items
    ADD CONSTRAINT fk_order_items_order FOREIGN KEY (order_id) REFERENCES public.orders(order_id) ON UPDATE CASCADE ON DELETE CASCADE;


--
-- TOC entry 4967 (class 2606 OID 18677)
-- Name: order_items fk_order_items_product; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.order_items
    ADD CONSTRAINT fk_order_items_product FOREIGN KEY (product_article) REFERENCES public.products(article) ON UPDATE CASCADE ON DELETE RESTRICT;


--
-- TOC entry 4964 (class 2606 OID 18650)
-- Name: orders fk_orders_pickup_point; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.orders
    ADD CONSTRAINT fk_orders_pickup_point FOREIGN KEY (pickup_point_id) REFERENCES public.pickup_points(pickup_point_id) ON UPDATE CASCADE ON DELETE RESTRICT;


--
-- TOC entry 4965 (class 2606 OID 18655)
-- Name: orders fk_orders_status; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.orders
    ADD CONSTRAINT fk_orders_status FOREIGN KEY (status_id) REFERENCES public.order_statuses(status_id) ON UPDATE CASCADE ON DELETE RESTRICT;


--
-- TOC entry 4960 (class 2606 OID 18583)
-- Name: products fk_products_category; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.products
    ADD CONSTRAINT fk_products_category FOREIGN KEY (category_id) REFERENCES public.categories(category_id) ON UPDATE CASCADE ON DELETE RESTRICT;


--
-- TOC entry 4961 (class 2606 OID 18578)
-- Name: products fk_products_manufacturer; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.products
    ADD CONSTRAINT fk_products_manufacturer FOREIGN KEY (manufacturer_id) REFERENCES public.manufacturers(manufacturer_id) ON UPDATE CASCADE ON DELETE RESTRICT;


--
-- TOC entry 4962 (class 2606 OID 18573)
-- Name: products fk_products_supplier; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.products
    ADD CONSTRAINT fk_products_supplier FOREIGN KEY (supplier_id) REFERENCES public.suppliers(supplier_id) ON UPDATE CASCADE ON DELETE RESTRICT;


--
-- TOC entry 4963 (class 2606 OID 18568)
-- Name: products fk_products_unit; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.products
    ADD CONSTRAINT fk_products_unit FOREIGN KEY (unit_id) REFERENCES public.units(unit_id) ON UPDATE CASCADE ON DELETE RESTRICT;


-- Completed on 2026-11-19 14:29:10

--
-- PostgreSQL database dump complete
--

\unrestrict OYhv5gGjtuamO8hdyJ8R9sYUBxzmn8fLkWiNlAhcGt6PIJj7xPqYcrTw2xi9H8K

