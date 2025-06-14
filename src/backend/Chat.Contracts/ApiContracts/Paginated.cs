﻿namespace Chat.Contracts.ApiContracts
{
    public record Paginated<TData>
    {
        /// <summary>
        /// Флаг, указывающий, имеет ли текущая страница предыдущую.
        /// </summary>
        public bool HasPreveiwPage { get; init; }

        /// <summary>
        /// Флаг, указывающий, имеет ли текущая страница следующую.
        /// </summary>
        public bool HasNextPage { get; init; }

        /// <summary>
        /// Всего страниц.
        /// </summary>
        public int TotalPages { get; init; }

        /// <summary>
        /// Неизменяемая коллекция, содержащая <typeparamref name="TData"/>.
        /// </summary>
        public required IReadOnlyCollection<TData> Items { get; init; }
    }

}
