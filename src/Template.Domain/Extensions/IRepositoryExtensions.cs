﻿namespace Template.Domain.Repositories
{
   
    using Entities.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Diagnostics.Contracts;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Provides extension methods for the <see cref="IReadOnlyRepository{T}"/> and <see cref="IRepository{T}"/> interfaces.
    /// </summary>
    public static class IRepositoryExtensions
    {
        /// <summary>
        /// Retrieves all items in the repository satisfied by the specified query asynchronously.
        /// </summary>
        /// <typeparam name="T">The <see cref="Type">type</see> of item in the repository.</typeparam>
        /// <param name="repository">The extended <see cref="IReadOnlyRepository{T}">repository</see>.</param>
        /// <param name="queryShaper">The <see cref="Func{T,TResult}">function</see> that shapes the <see cref="IQueryable{T}">query</see> to execute.</param>
        /// <returns>A <see cref="Task{T}">task</see> containing the retrieved <see cref="IEnumerable{T}">sequence</see>
        /// of <typeparamref name="T">items</typeparamref>.</returns>
        /// <example>The following example demonstrates how to paginate the items in a repository using an example query.
        /// <code><![CDATA[
        /// using Microsoft.DesignPatterns.Examples;
        /// using System;
        /// using System.Collections.Generic;
        /// using System.Linq;
        /// using System.Threading;
        /// using System.Threading.Tasks;
        /// 
        /// public async static void Main()
        /// {
        ///     var index = 0;
        ///     var repository = new MyRepository();
        ///     
        ///     foreach ( var item in await repository.GetAsync( q => q.Where( i => i.FirstName.StartsWith( "Jo" ) ).OrderBy( i => i.LastName ) );
        ///         Console.WriteLine( i => i.ToString() );
        /// }
        /// ]]>
        /// </code></example>
        [SuppressMessage( "Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Required for generics support." )]
        [SuppressMessage( "Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Validated by a code contract." )]
        public static Task<IEnumerable<TEntity>> GetAsync<TEntity, TKey>( this IReadOnlyRepository<TEntity, TKey> repository, Func<IQueryable<TEntity>, IQueryable<TEntity>> queryShaper ) where TEntity : class, IEntity<TKey> where TKey : IEquatable<TKey>
        {
            Contract.Requires( repository != null );
            Contract.Requires( queryShaper != null );
            Contract.Ensures( Contract.Result<Task<IEnumerable<TEntity>>>() != null );
            return repository.GetAsync( queryShaper, CancellationToken.None );
        }

        /// <summary>
        /// Retrieves a query result asynchronously.
        /// </summary>
        /// <typeparam name="T">The <see cref="Type">type</see> of item in the repository.</typeparam>
        /// <typeparam name="TResult">The <see cref="Type">type</see> of result to retrieve.</typeparam>
        /// <param name="repository">The extended <see cref="IReadOnlyRepository{T}">repository</see>.</param>
        /// <param name="queryShaper">The <see cref="Func{T,TResult}">function</see> that shapes the <see cref="IQueryable{T}">query</see> to execute.</param>
        /// <returns>A <see cref="Task{T}">task</see> containing the <typeparamref name="TResult">result</typeparamref> of the operation.</returns>
        [SuppressMessage( "Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Required for generics support." )]
        [SuppressMessage( "Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Validated by a code contract." )]
        public static Task<TResult> GetAsync<TEntity, TKey, TResult>( this IReadOnlyRepository<TEntity, TKey> repository, Func<IQueryable<TEntity>, TResult> queryShaper ) where TEntity : class, IEntity<TKey> where TKey : IEquatable<TKey>
        {
            Contract.Requires( repository != null );
            Contract.Requires( queryShaper != null );
            Contract.Ensures( Contract.Result<Task<TResult>>() != null );
            return repository.GetAsync( queryShaper, CancellationToken.None );
        }

        /// <summary>
        /// Retrieves all items in the repository asynchronously.
        /// </summary>
        /// <typeparam name="T">The <see cref="Type">type</see> of item in the repository.</typeparam>
        /// <param name="repository">The extended <see cref="IReadOnlyRepository{T}">repository</see>.</param>
        /// <returns>A <see cref="Task{T}">task</see> containing the <see cref="IEnumerable{T}">sequence</see>
        /// of all <typeparamref name="T">items</typeparamref> in the repository.</returns>
        /// <example>The following example demonstrates how to retrieve all items in a repository.
        /// <code><![CDATA[
        /// using Microsoft.DesignPatterns.Examples;
        /// using System;
        /// using System.Collections.Generic;
        /// using System.Linq;
        /// using System.Threading;
        /// using System.Threading.Tasks;
        /// 
        /// public async static void Main()
        /// {
        ///     var repository = new MyRepository();
        ///     
        ///     foreach ( var item in await repository.GetAllAsync( i => i.LastName == "Doe" ) )
        ///         Console.WriteLine( item.ToString() );
        /// }
        /// ]]>
        /// </code></example>
        [SuppressMessage( "Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Required for generics support." )]
        [SuppressMessage( "Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Validated by a code contract." )]
        public static Task<IEnumerable<TEntity>> GetAllAsync<TEntity, TKey>( this IReadOnlyRepository<TEntity, TKey> repository ) where TEntity : class, IEntity<TKey> where TKey : IEquatable<TKey>
        {
            Contract.Requires( repository != null );
            Contract.Ensures( Contract.Result<Task<IEnumerable<TEntity>>>() != null );
            return repository.GetAsync( q => q, CancellationToken.None );
        }

        /// <summary>
        /// Retrieves all items in the repository asynchronously.
        /// </summary>
        /// <typeparam name="T">The <see cref="Type">type</see> of item in the repository.</typeparam>
        /// <param name="repository">The extended <see cref="IReadOnlyRepository{T}">repository</see>.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken">cancellation token</see> that can be used to cancel the operation.</param>
        /// <returns>A <see cref="Task{T}">task</see> containing the <see cref="IEnumerable{T}">sequence</see>
        /// of all <typeparamref name="T">items</typeparamref> in the repository.</returns>
        /// <example>The following example demonstrates how to retrieve all items in a repository.
        /// <code><![CDATA[
        /// using Microsoft.DesignPatterns.Examples;
        /// using System;
        /// using System.Collections.Generic;
        /// using System.Linq;
        /// using System.Threading;
        /// using System.Threading.Tasks;
        /// 
        /// public async static void Main()
        /// {
        ///     var repository = new MyRepository();
        ///     var cancellationToken = new CancellationToken();
        ///     
        ///     foreach ( var item in await repository.GetAllAsync( i => i.LastName == "Doe", cancellationToken ) )
        ///         Console.WriteLine( item.ToString() );
        /// }
        /// ]]>
        /// </code></example>
        [SuppressMessage( "Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Required for generics support." )]
        [SuppressMessage( "Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Validated by a code contract." )]
        public static Task<IEnumerable<TEntity>> GetAllAsync<TEntity, TKey>( this IReadOnlyRepository<TEntity, TKey> repository, CancellationToken cancellationToken ) where TEntity : class, IEntity<TKey> where TKey : IEquatable<TKey>
        {
            Contract.Requires( repository != null );
            Contract.Ensures( Contract.Result<Task<IEnumerable<TEntity>>>() != null );
            return repository.GetAsync( q => q, cancellationToken );
        }

        /// <summary>
        /// Searches for items in the repository that match the specified predicate asynchronously.
        /// </summary>
        /// <typeparam name="T">The <see cref="Type">type</see> of item in the repository.</typeparam>
        /// <param name="repository">The extended <see cref="IReadOnlyRepository{T}">repository</see>.</param>
        /// <param name="predicate">The <see cref="Expression{T}">expression</see> representing the predicate used to
        /// match the requested <typeparamref name="T">items</typeparamref>.</param>
        /// <returns>A <see cref="Task{T}">task</see> containing the matched <see cref="IEnumerable{T}">sequence</see>
        /// of <typeparamref name="T">items</typeparamref>.</returns>
        /// <example>The following example demonstrates how to find items in a repository.
        /// <code><![CDATA[
        /// using Microsoft.DesignPatterns.Examples;
        /// using System;
        /// using System.Collections.Generic;
        /// using System.Linq;
        /// using System.Threading;
        /// using System.Threading.Tasks;
        /// 
        /// public async static void Main()
        /// {
        ///     var repository = new MyRepository();
        ///     var items = await repository.FindByAsync( i => i.LastName == "Doe" );
        ///     items.ForEach( i => Console.WriteLine( i.ToString() ) );
        /// }
        /// ]]>
        /// </code></example>
        [SuppressMessage( "Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Required for generics support." )]
        [SuppressMessage( "Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Validated by a code contract." )]
        public static Task<IEnumerable<TEntity>> FindByAsync<TEntity, TKey>( this IReadOnlyRepository<TEntity, TKey> repository, Expression<Func<TEntity, bool>> predicate ) where TEntity : class, IEntity<TKey> where TKey : IEquatable<TKey>
        {
            Contract.Requires( repository != null );
            Contract.Requires( predicate != null );
            Contract.Ensures( Contract.Result<Task<IEnumerable<TEntity>>>() != null );
            return repository.GetAsync( q => q.Where( predicate ), CancellationToken.None );
        }

        /// <summary>
        /// Searches for items in the repository that match the specified predicate asynchronously.
        /// </summary>
        /// <typeparam name="T">The <see cref="Type">type</see> of item in the repository.</typeparam>
        /// <param name="repository">The extended <see cref="IReadOnlyRepository{T}">repository</see>.</param>
        /// <param name="predicate">The <see cref="Expression{T}">expression</see> representing the predicate used to
        /// match the requested <typeparamref name="T">items</typeparamref>.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken">cancellation token</see> that can be used to cancel the operation.</param>
        /// <returns>A <see cref="Task{T}">task</see> containing the matched <see cref="IEnumerable{T}">sequence</see>
        /// of <typeparamref name="T">items</typeparamref>.</returns>
        /// <example>The following example demonstrates how to find items in a repository.
        /// <code><![CDATA[
        /// using Microsoft.DesignPatterns.Examples;
        /// using System;
        /// using System.Collections.Generic;
        /// using System.Linq;
        /// using System.Threading;
        /// using System.Threading.Tasks;
        /// 
        /// public async static void Main()
        /// {
        ///     var repository = new MyRepository();
        ///     var cancellationToken = new CancellationToken();
        ///     var items = await repository.FindByAsync( i => i.LastName == "Doe", cancellationToken );
        ///     items.ForEach( i => Console.WriteLine( i.ToString() ) );
        /// }
        /// ]]>
        /// </code></example>
        [SuppressMessage( "Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Required for generics support." )]
        [SuppressMessage( "Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Validated by a code contract." )]
        public static Task<IEnumerable<TEntity>> FindByAsync<TEntity, TKey>( this IReadOnlyRepository<TEntity, TKey> repository, Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken ) where TEntity : class, IEntity<TKey> where TKey : IEquatable<TKey>
        {
            Contract.Requires( repository != null );
            Contract.Requires( predicate != null );
            Contract.Ensures( Contract.Result<Task<IEnumerable<TEntity>>>() != null );
            return repository.GetAsync( q => q.Where( predicate ), cancellationToken );
        }

        /// <summary>
        /// Searches for items in the repository that match the Id asynchronously.
        /// </summary>
        /// <typeparam name="T">The <see cref="Type">type</see> of item in the repository.</typeparam>
        /// <param name="repository">The extended <see cref="IReadOnlyRepository{T}">repository</see>.</param>
        /// <param name="Id">The Id to search
        /// match the requested <typeparamref name="T">items</typeparamref>.</param>
        /// <returns>A <see cref="Task{T}">task</see> containing the matched <see cref="IEnumerable{T}">sequence</see>
        /// of <typeparamref name="T">items</typeparamref>.</returns>
        /// <example>The following example demonstrates how to find items in a repository.
        /// <code><![CDATA[
        /// using Microsoft.DesignPatterns.Examples;
        /// using System;
        /// using System.Collections.Generic;
        /// using System.Linq;
        /// using System.Threading;
        /// using System.Threading.Tasks;
        /// 
        /// public async static void Main()
        /// {
        ///     var repository = new MyRepository();
        ///     var items = await repository.FindByAsync( i => i.LastName == "Doe" );
        ///     items.ForEach( i => Console.WriteLine( i.ToString() ) );
        /// }
        /// ]]>
        /// </code></example>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Required for generics support.")]
        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Validated by a code contract.")]
        public static Task<TEntity> FindByIdAsync<TEntity, TKey>(this IReadOnlyRepository<TEntity, TKey> repository, TKey Id) where TEntity : class, IEntity<TKey> where TKey : IEquatable<TKey>
        {
            Contract.Requires(repository != null);
            Contract.Requires(Id != null);
            Contract.Ensures(Contract.Result<Task<IEnumerable<TEntity>>>() != null);
            return repository.GetAsync(q => q.SingleOrDefault(x=>x.Id.Equals(Id)), CancellationToken.None);
        }

        /// <summary>
        /// Searches for items in the repository that match the specified predicate asynchronously.
        /// </summary>
        /// <typeparam name="T">The <see cref="Type">type</see> of item in the repository.</typeparam>
        /// <param name="repository">The extended <see cref="IReadOnlyRepository{T}">repository</see>.</param>
        /// <param name="Id">The Id to search
        /// match the requested <typeparamref name="T">items</typeparamref>.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken">cancellation token</see> that can be used to cancel the operation.</param>
        /// <returns>A <see cref="Task{T}">task</see> containing the matched <see cref="IEnumerable{T}">sequence</see>
        /// of <typeparamref name="T">items</typeparamref>.</returns>
        /// <example>The following example demonstrates how to find items in a repository.
        /// <code><![CDATA[
        /// using Microsoft.DesignPatterns.Examples;
        /// using System;
        /// using System.Collections.Generic;
        /// using System.Linq;
        /// using System.Threading;
        /// using System.Threading.Tasks;
        /// 
        /// public async static void Main()
        /// {
        ///     var repository = new MyRepository();
        ///     var cancellationToken = new CancellationToken();
        ///     var items = await repository.FindByAsync( i => i.LastName == "Doe", cancellationToken );
        ///     items.ForEach( i => Console.WriteLine( i.ToString() ) );
        /// }
        /// ]]>
        /// </code></example>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Required for generics support.")]
        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Validated by a code contract.")]
        public static Task<TEntity> FindByIdAsync<TEntity, TKey>(this IReadOnlyRepository<TEntity, TKey> repository, TKey Id, CancellationToken cancellationToken) where TEntity : class, IEntity<TKey> where TKey : IEquatable<TKey>
        {
            Contract.Requires(repository != null);
            Contract.Requires(Id != null);
            Contract.Ensures(Contract.Result<Task<IEnumerable<TEntity>>>() != null);
            return repository.GetAsync(q => q.SingleOrDefault(x => x.Id.Equals(Id)), cancellationToken);
        }


        /// <summary>
        /// Searches for items in the repository that match the Ids asynchronously.
        /// </summary>
        /// <typeparam name="T">The <see cref="Type">type</see> of item in the repository.</typeparam>
        /// <param name="repository">The extended <see cref="IReadOnlyRepository{T}">repository</see>.</param>
        /// <param name="Ids">The Ids to search
        /// match the requested <typeparamref name="T">items</typeparamref>.</param>
        /// <returns>A <see cref="Task{T}">task</see> containing the matched <see cref="IEnumerable{T}">sequence</see>
        /// of <typeparamref name="T">items</typeparamref>.</returns>
        /// <example>The following example demonstrates how to find items in a repository.
        /// <code><![CDATA[
        /// using Microsoft.DesignPatterns.Examples;
        /// using System;
        /// using System.Collections.Generic;
        /// using System.Linq;
        /// using System.Threading;
        /// using System.Threading.Tasks;
        /// 
        /// public async static void Main()
        /// {
        ///     var repository = new MyRepository();
        ///     var items = await repository.FindByAsync( i => i.LastName == "Doe" );
        ///     items.ForEach( i => Console.WriteLine( i.ToString() ) );
        /// }
        /// ]]>
        /// </code></example>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Required for generics support.")]
        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Validated by a code contract.")]
        public static Task<IEnumerable<TEntity>> FindManyByIdAsync<TEntity, TKey>(this IReadOnlyRepository<TEntity, TKey> repository, IEnumerable<TKey> Ids) where TEntity : class, IEntity<TKey> where TKey : IEquatable<TKey>
        {
            Contract.Requires(repository != null);
            Contract.Requires(Ids != null);
            Contract.Ensures(Contract.Result<Task<IEnumerable<TEntity>>>() != null);
            return repository.GetAsync(q => q.Where(x => Ids.Contains(x.Id)), CancellationToken.None);
        }

        /// <summary>
        /// Searches for items in the repository that match the Ids asynchronously.
        /// </summary>
        /// <typeparam name="T">The <see cref="Type">type</see> of item in the repository.</typeparam>
        /// <param name="repository">The extended <see cref="IReadOnlyRepository{T}">repository</see>.</param>
        /// <param name="Ids">The Ids to search
        /// match the requested <typeparamref name="T">items</typeparamref>.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken">cancellation token</see> that can be used to cancel the operation.</param>
        /// <returns>A <see cref="Task{T}">task</see> containing the matched <see cref="IEnumerable{T}">sequence</see>
        /// of <typeparamref name="T">items</typeparamref>.</returns>
        /// <example>The following example demonstrates how to find items in a repository.
        /// <code><![CDATA[
        /// using Microsoft.DesignPatterns.Examples;
        /// using System;
        /// using System.Collections.Generic;
        /// using System.Linq;
        /// using System.Threading;
        /// using System.Threading.Tasks;
        /// 
        /// public async static void Main()
        /// {
        ///     var repository = new MyRepository();
        ///     var cancellationToken = new CancellationToken();
        ///     var items = await repository.FindByAsync( i => i.LastName == "Doe", cancellationToken );
        ///     items.ForEach( i => Console.WriteLine( i.ToString() ) );
        /// }
        /// ]]>
        /// </code></example>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Required for generics support.")]
        [SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Validated by a code contract.")]
        public static Task<IEnumerable<TEntity>> FindManyByIdAsync<TEntity, TKey>(this IReadOnlyRepository<TEntity, TKey> repository, IEnumerable<TKey> Ids, CancellationToken cancellationToken) where TEntity : class, IEntity<TKey> where TKey : IEquatable<TKey>
        {
            Contract.Requires(repository != null);
            Contract.Requires(Ids != null);
            Contract.Ensures(Contract.Result<Task<IEnumerable<TEntity>>>() != null);
            return repository.GetAsync(q => q.Where(x => Ids.Contains(x.Id)), cancellationToken);
        }


        /// <summary>
        /// Retrieves a single item in the repository matching the specified predicate asynchronously.
        /// </summary>
        /// <typeparam name="T">The <see cref="Type">type</see> of item in the repository.</typeparam>
        /// <param name="repository">The extended <see cref="IReadOnlyRepository{T}">repository</see>.</param>
        /// <param name="predicate">The <see cref="Expression{T}">expression</see> representing the predicate used to
        /// match the requested <typeparamref name="T">item</typeparamref>.</param>
        /// <returns>A <see cref="Task{T}">task</see> containing the matched <typeparamref name="T">item</typeparamref>
        /// or null if no match was found.</returns>
        /// <example>The following example demonstrates how to retrieve a single item from a repository.
        /// <code><![CDATA[
        /// using Microsoft.DesignPatterns.Examples;
        /// using System;
        /// using System.Collections.Generic;
        /// using System.Linq;
        /// using System.Threading;
        /// using System.Threading.Tasks;
        /// 
        /// public async static void Main()
        /// {
        ///     var repository = new MyRepository();
        ///     var item = await repository.GetSingleAsync( i => i.Id == 1 );
        ///     Console.WriteLine( item.ToString() );
        /// }
        /// ]]>
        /// </code></example>
        [SuppressMessage( "Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Required for generics support." )]
        [SuppressMessage( "Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Validated by a code contract." )]
        public static Task<TEntity> GetSingleAsync<TEntity, TKey>( this IReadOnlyRepository<TEntity, TKey> repository, Expression<Func<TEntity, bool>> predicate ) where TEntity : class, IEntity<TKey> where TKey : IEquatable<TKey>
        {
            Contract.Requires( repository != null );
            Contract.Requires( predicate != null );
            Contract.Ensures( Contract.Result<Task<TEntity>>() != null );
            return repository.GetSingleAsync( predicate, CancellationToken.None );
        }

        /// <summary>
        /// Retrieves a single item in the repository matching the specified predicate asynchronously.
        /// </summary>
        /// <typeparam name="T">The <see cref="Type">type</see> of item in the repository.</typeparam>
        /// <param name="repository">The extended <see cref="IReadOnlyRepository{T}">repository</see>.</param>
        /// <param name="predicate">The <see cref="Expression{T}">expression</see> representing the predicate used to
        /// match the requested <typeparamref name="T">item</typeparamref>.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken">cancellation token</see> that can be used to cancel the operation.</param>
        /// <returns>A <see cref="Task{T}">task</see> containing the matched <typeparamref name="T">item</typeparamref>
        /// or null if no match was found.</returns>
        /// <example>The following example demonstrates how to retrieve a single item from a repository.
        /// <code><![CDATA[
        /// using Microsoft.DesignPatterns.Examples;
        /// using System;
        /// using System.Collections.Generic;
        /// using System.Linq;
        /// using System.Threading;
        /// using System.Threading.Tasks;
        /// 
        /// public async static void Main()
        /// {
        ///     var repository = new MyRepository();
        ///     var item = await repository.GetSingleAsync( i => i.Id == 1 );
        ///     Console.WriteLine( item.ToString() );
        /// }
        /// ]]>
        /// </code></example>
        [SuppressMessage( "Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Required for generics support." )]
        [SuppressMessage( "Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Validated by a code contract." )]
        public static async Task<TEntity> GetSingleAsync<TEntity, TKey>( this IReadOnlyRepository<TEntity, TKey> repository, Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken ) where TEntity : class, IEntity<TKey> where TKey : IEquatable<TKey>
        {
            Contract.Requires( repository != null );
            Contract.Requires( predicate != null );
            Contract.Ensures( Contract.Result<Task<TEntity>>() != null );
            var items = await repository.GetAsync( q => q.Where( predicate ), cancellationToken );
            return items.SingleOrDefault();
        }

        /// <summary>
        /// Retrieves and pages all items in the repository asynchronously.
        /// </summary>
        /// <typeparam name="T">The <see cref="Type">type</see> of item in the repository.</typeparam>
        /// <param name="repository">The extended <see cref="IReadOnlyRepository{T}">repository</see>.</param>
        /// <param name="pageIndex">The zero-based index of the data page to retrieve.</param>
        /// <param name="pageSize">The size of the data page to retrieve.</param>
        /// <returns>A <see cref="Task{T}">task</see> containing the <see cref="PagedCollection{T}">paged collection</see>
        /// of <typeparamref name="T">items</typeparamref>.</returns>
        /// <example>The following example demonstrates how to paginate all items in a repository.
        /// <code><![CDATA[
        /// using Microsoft.DesignPatterns.Examples;
        /// using System;
        /// using System.Collections.Generic;
        /// using System.Linq;
        /// using System.Threading;
        /// using System.Threading.Tasks;
        /// 
        /// public async static void Main()
        /// {
        ///     var index = 0;
        ///     var repository = new MyRepository();
        ///     var items = await repository.PaginateAsync( index, 10 );
        ///     var retrieved = items.Count;
        ///     
        ///     items.ForEach( i => Console.WriteLine( i.ToString() ) );
        ///     
        ///     while ( retrieved < items.TotalCount )
        ///     {
        ///         items = await repository.PaginateAsync( ++index, 10 );
        ///         retrieved += items.Count;
        ///         items.ForEach( i => Console.WriteLine( i.ToString() ) );
        ///     }
        /// }
        /// ]]>
        /// </code></example>
        [SuppressMessage( "Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Required for generics support." )]
        [SuppressMessage( "Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Validated by a code contract." )]
        public static Task<PagedCollection<TEntity>> PaginateAsync<TEntity, TKey>( this IReadOnlyRepository<TEntity, TKey> repository, int pageIndex, int pageSize ) where TEntity : class, IEntity<TKey> where TKey : IEquatable<TKey>
        {
            Contract.Requires( repository != null );
            Contract.Requires( pageIndex >= 0 );
            Contract.Requires( pageSize >= 1 );
            Contract.Ensures( Contract.Result<Task<PagedCollection<TEntity>>>() != null );
            return repository.PaginateAsync( pageIndex, pageSize, CancellationToken.None );
        }

        /// <summary>
        /// Retrieves and pages all items in the repository asynchronously.
        /// </summary>
        /// <typeparam name="T">The <see cref="Type">type</see> of item in the repository.</typeparam>
        /// <param name="repository">The extended <see cref="IReadOnlyRepository{T}">repository</see>.</param>
        /// <param name="pageIndex">The zero-based index of the data page to retrieve.</param>
        /// <param name="pageSize">The size of the data page to retrieve.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken">cancellation token</see> that can be used to cancel the operation.</param>
        /// <returns>A <see cref="Task{T}">task</see> containing the <see cref="PagedCollection{T}">paged collection</see>
        /// of <typeparamref name="T">items</typeparamref>.</returns>
        /// <example>The following example demonstrates how to paginate all items in a repository.
        /// <code><![CDATA[
        /// using Microsoft.DesignPatterns.Examples;
        /// using System;
        /// using System.Collections.Generic;
        /// using System.Linq;
        /// using System.Threading;
        /// using System.Threading.Tasks;
        /// 
        /// public async static void Main()
        /// {
        ///     var index = 0;
        ///     var repository = new MyRepository();
        ///     var cancellationToken = new CancellationToken();
        ///     var items = await repository.PaginateAsync( index, 10, cancellationToken );
        ///     var retrieved = items.Count;
        ///     
        ///     items.ForEach( i => Console.WriteLine( i.ToString() ) );
        ///     
        ///     while ( retrieved < items.TotalCount )
        ///     {
        ///         items = await repository.PaginateAsync( ++index, 10, cancellationToken );
        ///         retrieved += items.Count;
        ///         items.ForEach( i => Console.WriteLine( i.ToString() ) );
        ///     }
        /// }
        /// ]]>
        /// </code></example>
        [SuppressMessage( "Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Justification = "Required to support paging." )]
        [SuppressMessage( "Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Required for generics support." )]
        [SuppressMessage( "Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Validated by a code contract." )]
        public static async Task<PagedCollection<TEntity>> PaginateAsync<TEntity, TKey>( this IReadOnlyRepository<TEntity, TKey> repository, int pageIndex, int pageSize, CancellationToken cancellationToken ) where TEntity : class, IEntity<TKey> where TKey : IEquatable<TKey>
        {
            Contract.Requires( repository != null );
            Contract.Requires( pageIndex >= 0 );
            Contract.Requires( pageSize >= 1 );
            Contract.Ensures( Contract.Result<Task<PagedCollection<TEntity>>>() != null );

            var groups = await repository.GetAsync(
                q =>
                {
                    var startIndex = pageIndex * pageSize;
                    return q.Skip( startIndex )
                            .Take( pageSize )
                            .GroupBy(
                                g => new
                                {
                                    Total = q.Count()
                                } );
                },
                cancellationToken );

            // return first group
            var result = groups.FirstOrDefault();

            if ( result == null )
                return new PagedCollection<TEntity>( Enumerable.Empty<TEntity>(), 0L );

            return new PagedCollection<TEntity>( result, result.Key.Total );
        }

        /// <summary>
        /// Retrieves and pages matching items in the repository asynchronously.
        /// </summary>
        /// <typeparam name="T">The <see cref="Type">type</see> of item in the repository.</typeparam>
        /// <param name="repository">The extended <see cref="IReadOnlyRepository{T}">repository</see>.</param>
        /// <param name="queryShaper">The <see cref="Func{T,TResult}">function</see> that shapes the <see cref="IQueryable{T}">query</see> to execute.</param>
        /// <param name="pageIndex">The zero-based index of the data page to retrieve.</param>
        /// <param name="pageSize">The size of the data page to retrieve.</param>
        /// <returns>A <see cref="Task{T}">task</see> containing the <see cref="PagedCollection{T}">paged collection</see>
        /// of <typeparamref name="T">items</typeparamref>.</returns>
        /// <example>The following example demonstrates how to paginate all items in a repository.
        /// <code><![CDATA[
        /// using Microsoft.DesignPatterns.Examples;
        /// using System;
        /// using System.Collections.Generic;
        /// using System.Linq;
        /// using System.Threading;
        /// using System.Threading.Tasks;
        /// 
        /// public async static void Main()
        /// {
        ///     var index = 0;
        ///     var repository = new MyRepository();
        ///     var items = await repository.PaginateAsync( q => q.OrderBy( i => LastName ), index, 10 );
        ///     var retrieved = items.Count;
        ///     
        ///     items.ForEach( i => Console.WriteLine( i.ToString() ) );
        ///     
        ///     while ( retrieved < items.TotalCount )
        ///     {
        ///         items = await repository.PaginateAsync( q => q.OrderBy( i => LastName ), ++index, 10 );
        ///         retrieved += items.Count;
        ///         items.ForEach( i => Console.WriteLine( i.ToString() ) );
        ///     }
        /// }
        /// ]]>
        /// </code></example>
        [SuppressMessage( "Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Required for generics support." )]
        [SuppressMessage( "Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Validated by a code contract." )]
        public static Task<PagedCollection<TEntity>> PaginateAsync<TEntity, TKey>( this IReadOnlyRepository<TEntity, TKey> repository, Func<IQueryable<TEntity>, IQueryable<TEntity>> queryShaper, int pageIndex, int pageSize ) where TEntity : class, IEntity<TKey> where TKey : IEquatable<TKey>
        {
            Contract.Requires( repository != null );
            Contract.Requires( pageIndex >= 0 );
            Contract.Requires( pageSize >= 1 );
            Contract.Ensures( Contract.Result<Task<PagedCollection<TEntity>>>() != null );
            return repository.PaginateAsync( queryShaper, pageIndex, pageSize, CancellationToken.None );
        }

        /// <summary>
        /// Retrieves and pages matching items in the repository asynchronously.
        /// </summary>
        /// <typeparam name="T">The <see cref="Type">type</see> of item in the repository.</typeparam>
        /// <param name="repository">The extended <see cref="IReadOnlyRepository{T}">repository</see>.</param>
        /// <param name="queryShaper">The <see cref="Func{T,TResult}">function</see> that shapes the <see cref="IQueryable{T}">query</see> to execute.</param>
        /// <param name="pageIndex">The zero-based index of the data page to retrieve.</param>
        /// <param name="pageSize">The size of the data page to retrieve.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken">cancellation token</see> that can be used to cancel the operation.</param>
        /// <returns>A <see cref="Task{T}">task</see> containing the <see cref="PagedCollection{T}">paged collection</see>
        /// of <typeparamref name="T">items</typeparamref>.</returns>
        /// <example>The following example demonstrates how to paginate all items in a repository.
        /// <code><![CDATA[
        /// using Microsoft.DesignPatterns.Examples;
        /// using System;
        /// using System.Collections.Generic;
        /// using System.Linq;
        /// using System.Threading;
        /// using System.Threading.Tasks;
        /// 
        /// public async static void Main()
        /// {
        ///     var index = 0;
        ///     var repository = new MyRepository();
        ///     var cancellationToken = new CancellationToken();
        ///     var items = await repository.PaginateAsync( q => q.OrderBy( i => LastName ), index, 10, cancellationToken );
        ///     var retrieved = items.Count;
        ///     
        ///     items.ForEach( i => Console.WriteLine( i.ToString() ) );
        ///     
        ///     while ( retrieved < items.TotalCount )
        ///     {
        ///         items = await repository.PaginateAsync( q => q.OrderBy( i => LastName ), ++index, 10, cancellationToken );
        ///         retrieved += items.Count;
        ///         items.ForEach( i => Console.WriteLine( i.ToString() ) );
        ///     }
        /// }
        /// ]]>
        /// </code></example>
        [SuppressMessage( "Microsoft.Maintainability", "CA1506:AvoidExcessiveClassCoupling", Justification = "Required to support paging." )]
        [SuppressMessage( "Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Required for generics support." )]
        [SuppressMessage( "Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Validated by a code contract." )]
        public static async Task<PagedCollection<TEntity>> PaginateAsync<TEntity, TKey>( this IReadOnlyRepository<TEntity, TKey> repository, Func<IQueryable<TEntity>, IQueryable<TEntity>> queryShaper, int pageIndex, int pageSize, CancellationToken cancellationToken ) where TEntity : class, IEntity<TKey> where TKey : IEquatable<TKey>
        {
            Contract.Requires( repository != null );
            Contract.Requires( pageIndex >= 0 );
            Contract.Requires( pageSize >= 1 );
            Contract.Ensures( Contract.Result<Task<PagedCollection<TEntity>>>() != null );

            var groups = await repository.GetAsync(
                                q =>
                                {
                                    var query = queryShaper( q );
                                    var startIndex = pageIndex * pageSize;
                                    return query.Skip( startIndex )
                                                .Take( pageSize )
                                                .GroupBy(
                                                    g => new
                                                    {
                                                        Total = query.Count()
                                                    } );
                                },
                                cancellationToken );

            // return first group
            var result = groups.FirstOrDefault();

            if ( result == null )
                return new PagedCollection<TEntity>( Enumerable.Empty<TEntity>(), 0L );

            return new PagedCollection<TEntity>( result, result.Key.Total );
        }

        /// <summary>
        /// Saves all pending changes in the repository asynchronously.
        /// </summary>
        /// <typeparam name="T">The <see cref="Type">type</see> of item in the repository.</typeparam>
        /// <param name="repository">The extended <see cref="IReadOnlyRepository{T}">repository</see>.</param>
        /// <returns>A <see cref="Task">task</see> representing the save operation.</returns>
        [SuppressMessage( "Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Validated by a code contract." )]
        public static Task SaveChangesAsync<TEntity, TKey>( this IRepository<TEntity, TKey> repository) where TEntity : class, IEntity<TKey> where TKey : IEquatable<TKey>
        {
            Contract.Requires( repository != null );
            Contract.Ensures( Contract.Result<Task>() != null );
            return repository.SaveChangesAsync( CancellationToken.None );
        }
    }
}
