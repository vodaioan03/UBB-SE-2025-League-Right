-- Insert Users
INSERT INTO Users (Username) VALUES 
('geo_learner1'), 
('map_master'), 
('continent_king');

-- Insert Roadmaps
INSERT INTO Roadmaps (Name) VALUES 
('World Geography');

-- Insert Sections
INSERT INTO Sections (SubjectId, Title, Description, RoadmapId, OrderNumber) VALUES 
(1, 'Introduction to Continents', 'Learn the basics of continents', 1, 1),
(1, 'Countries of Europe', 'Explore the countries in Europe', 1, 2),
(1, 'Asian Capitals', 'Memorize the capitals of Asia', 1, 3);

-- Insert Quizzes
INSERT INTO Quizzes (SectionId, OrderNumber) VALUES 
(1, 1),
(2, 1),
(1, 2);

-- Insert Exercises
INSERT INTO Exercises (Type, DifficultyId, Question) VALUES 
('MultipleChoiceExercise', 1, 'What is the largest continent?'),
('FillInTheBlankExercise', 2, 'The capital of France is Paris.'),
('AssociationExercise', 3, 'Match the countries with their capitals.'),
('FlashcardExercise', 1, 'What is the capital of Japan?'),
('MultipleChoiceExercise', 2, 'Which continent is known as the "Dark Continent"?'),
('FillInTheBlankExercise', 1, 'The capital of Germany is Berlin.'),
('AssociationExercise', 2, 'Match the country with its capital.'),
('FlashcardExercise', 3, 'What is the capital of Italy?'),
('MultipleChoiceExercise', 3, 'Which continent is the smallest?'),
('FillInTheBlankExercise', 2, 'The capital of Spain is Madrid.'),
('AssociationExercise', 1, 'Match the country with its capital.'),
('FlashcardExercise', 2, 'What is the capital of Canada?');


-- Insert Multiple Choice Exercise Details
INSERT INTO MultipleChoiceExercises (ExerciseId, CorrectAnswer) VALUES 
(1, 'Asia'),
(5, 'Africa'), 
(9, 'Australia');

INSERT INTO MultipleChoiceOptions (ExerciseId, OptionText) VALUES 
(1, 'Africa'),
(1, 'Europe'),
(1, 'North America'),
(5, 'Antarctica'),
(5, 'Europe'),
(5, 'Australia'),
(9, 'Asia'),
(9, 'Europe'),
(9, 'North America');

-- Insert Fill In The Blank Answers
INSERT INTO FillInTheBlankExercises (ExerciseId) VALUES
(2),
(6), 
(10);

INSERT INTO FillInTheBlankAnswers (ExerciseId, CorrectAnswer) VALUES 
(2, 'Paris'),
(6, 'Berlin'), 
(10, 'Madrid');

-- Insert Association Exercise
INSERT INTO AssociationExercises (ExerciseId) VALUES
(3),
(7), 
(11);

INSERT INTO AssociationPairs (ExerciseId, FirstAnswer, SecondAnswer) VALUES 
(3, 'France', 'Paris'),
(3, 'Germany', 'Berlin'),
(3, 'Italy', 'Rome'),
(7, 'Spain', 'Madrid'),
(7, 'Italy', 'Rome'),
(11, 'Canada', 'Ottawa'),
(11, 'USA', 'Washington D.C.'),
(11, 'Mexico', 'Mexico City'),
(11, 'Brazil', 'Brasilia'),
(11, 'Argentina', 'Buenos Aires');

-- Insert Flashcard Exercise
INSERT INTO FlashcardExercises (ExerciseId, Answer) VALUES 
(4, 'Tokyo'),
(8, 'Rome'), 
(12, 'Ottawa');

-- Link Exercises to Quizzes
INSERT INTO QuizExercises (QuizId, ExerciseId) VALUES 
(1, 1),
(1, 2),
(2, 3),
(3, 4),
(3, 5),
(1, 6),
(2, 7),
(3, 8),
(1, 9),
(2, 10),
(3, 11),
(1, 12);

-- Insert Exam
INSERT INTO Exams (SectionId) VALUES (3);
DECLARE @examId INT = SCOPE_IDENTITY();

-- Link Exercises to Exam
INSERT INTO ExamExercises (ExamId, ExerciseId) VALUES 
(@examId, 1),
(@examId, 2),
(@examId, 3);

-- Update Users' last completed sections and quizzes
UPDATE Users SET LastCompletedSectionId = 1, LastCompletedQuizId = 1 WHERE Username = 'geo_learner1';
UPDATE Users SET LastCompletedSectionId = 2, LastCompletedQuizId = 2 WHERE Username = 'map_master';


--
DELETE FROM Exams WHERE Id = 3;