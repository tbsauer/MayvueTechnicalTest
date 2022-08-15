using Dapper;
using MotionPicturesCore.Context;
using MotionPicturesCore.Interfaces;
using MotionPicturesCore.Models;
using System.Data;
using System.Data.SqlClient;

namespace MotionPicturesCore.Services
{
    public class MotionPictureService : IMotionPictureService
    {
		private readonly DapperContext _context;
		public MotionPictureService(DapperContext context)
		{
			_context = context;
		}

		public async Task<IEnumerable<MotionPicture>> GetMotionPictures()
		{
			var procedureName = "MotionPictures_SelectAll";

			using (var connection = _context.CreateConnection())
			{
				var movies = await connection.QueryAsync<MotionPicture>(procedureName);
				return movies.ToList();
			}
		}

		public async Task<MotionPicture> GetMotionPicture(int id)
		{
			var procedureName = "MotionPictures_SelectById";
			var parameters = new DynamicParameters();
			parameters.Add("Id", id, DbType.Int32, ParameterDirection.Input);

			using (var connection = _context.CreateConnection())
			{
				var company = await connection.QuerySingleOrDefaultAsync<MotionPicture>(procedureName, parameters, commandType: CommandType.StoredProcedure);

				return company;
			}
		}

		public async Task<MotionPicture> CreateMotionPicture(MotionPictureAddRequest movie)
		{
			var procedureName = "MotionPictures_Insert";

			var parameters = new DynamicParameters();
			parameters.Add("Name", movie.Name, DbType.String);
			parameters.Add("Description", movie.Description, DbType.String);
			parameters.Add("ReleaseYear", movie.ReleaseYear, DbType.Int16);
			parameters.Add("DateCreated", movie.DateCreated, DbType.DateTime2);
			parameters.Add("CreatedBy", movie.CreatedBy, DbType.Int16);
			parameters.Add("Id", dbType: DbType.Int32, direction: ParameterDirection.Output);
			

			using (var connection = _context.CreateConnection())
			{
				var newId = await connection.QueryFirstOrDefaultAsync<int>(procedureName, parameters, commandType: CommandType.StoredProcedure);

				var createdMovie = new MotionPicture
				{
					Id = newId,
					Name = movie.Name,
					Description = movie.Description,
					ReleaseYear = movie.ReleaseYear,
					DateCreated = movie.DateCreated,
					CreatedBy = movie.CreatedBy
				};

				return createdMovie;
			}
		}

		public async Task UpdateMotionPicture(int id, MotionPictureUpdateRequest movie)
		{
			var procedureName = "MotionPictures_Update";

			var parameters = new DynamicParameters();
			parameters.Add("Id", id, DbType.Int32);
			parameters.Add("Name", movie.Name, DbType.String);
			parameters.Add("Description", movie.Description, DbType.String);
			parameters.Add("ReleaseYear", movie.ReleaseYear, DbType.Int16);
			parameters.Add("DateModified", movie.DateModified, DbType.DateTime2);
			parameters.Add("CreatedBy", movie.CreatedBy, DbType.Int32);

			using (var connection = _context.CreateConnection())
			{
				await connection.ExecuteAsync(procedureName, parameters, commandType: CommandType.StoredProcedure);
			}
		}

		public async Task DeleteMotionPicture(int id)
		{
			var procedureName = "MotionPictures_Delete";
			var parameters = new DynamicParameters();
			parameters.Add("Id", id, DbType.Int32);

			using (var connection = _context.CreateConnection())
			{
				await connection.ExecuteAsync(procedureName, parameters, commandType: CommandType.StoredProcedure);
			}
		}

	}

}

   
